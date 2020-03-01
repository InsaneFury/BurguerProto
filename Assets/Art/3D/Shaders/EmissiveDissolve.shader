Shader "Unlit/EmissiveDissolve"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		_HorizontalScroll("Horizontal Scroll", Vector) = (1, 0,0,0)

		_NoiseAScale("Noise A Scale", Float) = 21.7
		_NoiseAOffset("Noise A Offset", Vector) = (0,0,0,0)
		_NoiseAWeight("Noise A Weight", Float) = 0.5

		_NoiseBScale("Noise B Scale", Float) = 12
		_NoiseBOffset("Noise B Offset", Vector) = (0,0,0,0)
		_NoiseBWeight("Noise B Weight", Float) = 4

		_Cutout("Cutout", Range(0,1)) = 0.5
		_Width("Width", Range(0,1)) = 0.11
		[HDR] _Edge("Edge", Color) = (0,1.4,2.3,1)
	}

		SubShader
		{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
	#pragma vertex SpriteVert
	#pragma fragment SpriteFragCustom
	#pragma target 2.0
	#pragma multi_compile_instancing
	#pragma multi_compile _ PIXELSNAP_ON
	#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
	#include "UnitySprites.cginc"

			float4 _MainTex_TexelSize;
			float2 _HorizontalScroll;

			float   _NoiseAScale;
			float2  _NoiseAOffset;
			float   _NoiseAWeight;

			float   _NoiseBScale;
			float2  _NoiseBOffset;
			float   _NoiseBWeight;

			float _Cutout;
			float _Width;
			fixed4 _Edge;

			float2 unity_gradientNoise_dir(float2 p)
			{
				p = p % 289;
				float x = (34 * p.x + 1) * p.x % 289 + p.y;
				x = (34 * x + 1) * x % 289;
				x = frac(x / 41) * 2 - 1;
				return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
			}

			float unity_gradientNoise(float2 p)
			{
				float2 ip = floor(p);
				float2 fp = frac(p);
				float d00 = dot(unity_gradientNoise_dir(ip), fp);
				float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
				float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
				float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
				fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
				return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
			}

			float Unity_GradientNoise_float(float2 UV, float Scale)
			{
				return unity_gradientNoise(UV * Scale) + 0.5;
			}

			fixed4 SpriteFragCustom(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
			//pixelate uv
			float2 uv_p = floor(IN.texcoord * _MainTex_TexelSize.zw) / _MainTex_TexelSize.zw;

			//horizontal scroll
			float scroll_h = uv_p.x * (_HorizontalScroll.y - _HorizontalScroll.x) + _HorizontalScroll.x;

			//noise
			float noise_a = Unity_GradientNoise_float(uv_p + _NoiseAOffset, _NoiseAScale) * (_NoiseAWeight / (_NoiseAWeight + _NoiseBWeight));
			float noise_b = Unity_GradientNoise_float(uv_p + _NoiseBOffset, _NoiseBScale) * (_NoiseBWeight / (_NoiseAWeight + _NoiseBWeight));
			float noise = noise_a + noise_b;

			float cutout_base = noise * scroll_h;

			float cutout = _Cutout * (1 + _Width) - _Width;
			//dissolve
			float dissolve = step(cutout, cutout_base);

			//edge
			float edge = dissolve - step(cutout + _Width, cutout_base);

			fixed4 edge_c = _Edge * edge;
			dissolve *= c.a;
			c += edge_c;
			c *= dissolve;

			c.rgb *= c.a;
			return c;
		}
		ENDCG
	}
		}
}