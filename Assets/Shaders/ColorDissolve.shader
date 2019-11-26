Shader "Custom/ColorDissolve" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_Color("Start Color", Color) = (1,1,1,1)
		_TargetColor("Target Color", Color) = (1,1,1,1)
		_DissolveTex("Dissolve (grayscale)", 2D) = "white" {}
		_EdgeColor("Edge Color", Color) = (1,1,1,1)
		_EdgeSize("Edge Size", Range(0,1)) = 0.1
		_T("Lerp Time", Range(0,1)) = 0.0
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0
	}
		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
			LOD 200
			ZWrite On

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows alphatest:_Cutoff addshadow

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex, _DissolveTex;

			struct Input {
				float2 uv_MainTex;
				float2 uv_DissolveTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color, _TargetColor, _EdgeColor;
			half _T, _EdgeSize;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {
				fixed4 textureColor = tex2D(_MainTex, IN.uv_MainTex);

				fixed4 startColor = textureColor * _Color;
				fixed4 endColor = textureColor * _TargetColor;

				fixed4 dissolveColor = tex2D(_DissolveTex, IN.uv_DissolveTex);

				// Using red chanel of dissolve texture as grayscale value
				float dissolveTime = dissolveColor.r;

				fixed4 albedo = startColor;
				if (dissolveTime <= _T) {
					albedo = endColor;

					// The next bit of code calculates the brightness of the edge
					float edgeTime = (1 - _T) + dissolveTime;

					// Shrink edge at the end of the transition
					float edgeSize = min(_EdgeSize, (1 - _T) / _EdgeSize);

					edgeTime = clamp(edgeTime - (1 - edgeSize), 0.0, 1.0);

					// Fix edge case at _T = 1
					edgeTime /= max(edgeSize, 0.001);
					o.Emission = edgeTime * _EdgeColor;
				}

				o.Albedo = albedo.rgb;
				o.Alpha = albedo.a;

				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
			}
			ENDCG
		}
			FallBack "Diffuse"
}