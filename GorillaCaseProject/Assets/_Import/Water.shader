Shader "Custom/Water" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags {
			"Queue" = "TransParent"
			"RenderType"="Transparent"
		}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert alpha
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void vert(inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float amp = 0.05*sin(_Time*20 + v.vertex.x*20);
			v.vertex.xyz = float3(v.vertex.x, v.vertex.y + amp, v.vertex.z);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
//			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
//			o.Albedo = c.rgb;
			o.Albedo = float3(0.3f, 0.5f, 0.8f);
			o.Alpha = 0.5f;
		}
		ENDCG
	}
	FallBack "Diffuse"
}