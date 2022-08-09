Shader "CustomShaders/PointBased"
{
	Properties
	{
		_Smoothness("Smoothness",Range(0,1))=0.5
	}

	SubShader 
	{
		CGPROGRAM
		#pragma surface ConfigureSurfacePoint Standard fullforwardshadows
		#pragma target 3.0

		struct Input{
			float3 worldPos;
		};


		float _Smoothness;

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