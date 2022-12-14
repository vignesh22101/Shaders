Shader "CustomShaders/Point Surface GPU"
{
	Properties
	{
		_Smoothness("Smoothness",Range(0,1))=0.5
	}

	SubShader 
	{
		CGPROGRAM
		#pragma surface ConfigureSurfacePoint Standard fullforwardshadows addshadow
		#pragma target 4.5
		#pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural

		struct Input{
			float3 worldPos;
		};


		float _Smoothness;

		#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
			StructuredBuffer<float3> _Positions;
		#endif

		float _Step;

		void ConfigureProcedural () {
			#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
			float3 position = _Positions[unity_InstanceID];
			unity_ObjectToWorld=0.0;
			unity_ObjectToWorld._m03_m13_m23_m33=float4(position,1.0);
			unity_ObjectToWorld._m00_m11_m22=_Step;
			#endif
		}

		void ConfigureSurfacePoint(Input input,inout SurfaceOutputStandard surface)
		{
				surface.Albedo=saturate(input.worldPos*.5+.5);
				surface.Smoothness=_Smoothness;
				surface.Metallic=saturate(input.worldPos.y*.5+.5);
		}
		ENDCG
	}

	Fallback "Diffuse"
}