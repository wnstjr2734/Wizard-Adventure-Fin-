// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GAPH Custom Shader/Dissolve Texture/Dissolve Texture (SimpleFade)" {
	Properties {
		[HDR]_Color("For Red Color",Color) = (0.5, 0.5, 0.5, 0.5)
		[HDR]_TintColor2("For Green Color",Color) = (1,1,1,1)
		[HDR]_TintColor3("For Blue Color",Color) = (1,1,1,1)
		_MainTex ("Fade Texture", 2D) = "white" {}
		_Factor("Fade Texture Factor", float) = 1.0
		_CutOut("CutOut",Range(0,1)) = 0.5
		_InvFade("Soft Particle Factor", Range(0.01,3.0)) = 1.0

	}
		Category{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transperent" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			ZWrite Off

		SubShader {
				Pass{
					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 3.0
					#pragma multi_compile_instancing
					#pragma multi_compile_fog

					#include "UnityCG.cginc"

					UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);

					float _Factor;
					fixed4 _TintColor; 
					fixed4 _TintColor2;
					fixed4 _TintColor3;
					float _CutOut;

					struct appdata_t {
						float4 vertex : POSITION;
						float2 texcoord : TEXCOORD0;
					
						UNITY_VERTEX_INPUT_INSTANCE_ID //Insert	
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						float2 texcoord : TEXCOORD0;
						UNITY_FOG_COORDS(1)
						#ifdef SOFTPARTICLES_ON
							float4 projPos : TEXCOORD2;
						#endif

						UNITY_VERTEX_OUTPUT_STEREO //Insert
					};

					float4 _MainTex_ST;

					v2f vert(appdata_t v)
					{
						v2f o;

						UNITY_SETUP_INSTANCE_ID(v); //Insert
						UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert
													
						#ifdef SHADER_API_D3D11
							o.vertex = UnityObjectToClipPos(v.vertex);
						#else 
							o.vertex = UnityObjectToClipPos(v.vertex);
						#endif
							
						#ifdef SOFTPARTICLES_ON
							o.projPos = ComputeScreenPos(o.vertex);
							COMPUTE_EYEDEPTH(o.projPos.z);
						#endif	
						o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
						UNITY_TRANSFER_FOG(o, o.vertex);
						return o;
					}

					sampler2D _CameraDepthTexture;
					float _InvFade;

					fixed4 frag(v2f i) : SV_Target
					{
						UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

						#ifdef SOFTPARTICLES_ON
							float sceneZ = LinearEyeDepth(SAMPLER_DEPTH_TEXTURE_PROJ(_CameraDepthTexture,UNITY_PROJ_COORD(i.projPos)));
							float partZ = i.projPos.z;
							float fade = saturate(_InvFade * (sceneZ - partZ));
							i.color.a *= fade;
						#endif

						half4 tex = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.texcoord);
						//Cutout tex using every each color channel.
						half saturate_Color = tex.r - (_CutOut) + tex.g-(_CutOut) + tex.b - ( _CutOut); 
						//Set cutout  tex & cutout value to alpha using cutout state, tex info.
						half alpha_Value = (_CutOut-1) > (1 - tex ) ? tex * _Factor : saturate(saturate_Color); 

						half4 color = half4(0, 0, 0, 0);
						//Color compose with each color value.
						color += tex.r *_TintColor * _TintColor.a ; 
						color += tex.g *_TintColor2  * _TintColor2.a ;
						color += tex.b *_TintColor3  * _TintColor3.a ;
						half4 res = color * 2;

						res.a = alpha_Value;
						UNITY_APPLY_FOG_COLOR(i.fogCoord, res, half4(0, 0, 0, 0));
						return res;
					}
				ENDCG
			}
		}
	}
}