// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GAPH Custom Shader/Distortion Effect/Mobile/Mobile - Distortion Effect(WithMask)" {
	Properties {
			_TintColor ("Tint Color", Color) = (1,1,1,1)
			_Mask ("Mask (A)",2D) = "black"{}
			_NormalMap ("Normalmap", 2D) = "bump" {}
			_DistortFactor ("Distortion", Float) = 10
			_InvFade ("Soft Particles Factor", Range(0,10)) = 1.0
	}

	Category {

		Tags { "Queue"="Transparent"  "IgnoreProjector"="True"  "RenderType"="Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off 
		Lighting Off 
		ZWrite Off 

		SubShader {
			GrabPass {							
				Name "BASE"
				Tags { "LightMode" = "Always" }
			}
				Pass {
					Name "BASE"
					Tags { "LightMode" = "Always" }
					
					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma fragmentoption ARB_precision_hint_fastest
					#pragma multi_compile_instancing
					#pragma multi_compile_particles
					#include "UnityCG.cginc"

					struct appdata_t {
						float4 vertex : POSITION;
						float2 texcoord: TEXCOORD0;
						fixed4 color : COLOR;
					
						UNITY_VERTEX_INPUT_INSTANCE_ID //Insert	
					};

					struct v2f {
						float4 vertex : POSITION;
						float4 uvgrab : TEXCOORD0;
						float2 uvnormal : TEXCOORD1;
						float2 uvmask : TEXCOORD2;
						fixed4 color : COLOR;
						#ifdef SOFTPARTICLES_ON
							float4 projPos : TEXCOORD3;
						#endif

						UNITY_VERTEX_OUTPUT_STEREO //Insert
					};

					fixed4 _TintColor;

					UNITY_DECLARE_SCREENSPACE_TEXTURE(_Mask);
					UNITY_DECLARE_SCREENSPACE_TEXTURE(_NormalMap);
					UNITY_DECLARE_SCREENSPACE_TEXTURE(_GrabTextureMobile);

					float _DistortFactor;
					float _ColorFactor;			

					float4 _NormalMap_ST;
					float4 _Mask_ST;
					float4 _GrabTextureMobile_TexelSize;

					v2f vert (appdata_t v)
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
							o.projPos = ComputeScreenPos (o.vertex);
							COMPUTE_EYEDEPTH(o.projPos.z);
						#endif
						o.color = v.color;

						#if UNITY_UV_STARTS_AT_TOP
							float scale = -1.0;
						#else
							float scale = 1.0;
						#endif

						o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y) + o.vertex.w) * 0.5;
						o.uvgrab.zw = o.vertex.zw;

						o.uvnormal = TRANSFORM_TEX( v.texcoord, _NormalMap );
						o.uvmask = TRANSFORM_TEX(v.texcoord,_Mask);
						
						return o;
					}

					sampler2D _CameraDepthTexture;
					float _InvFade;

					half4 frag( v2f i ) : COLOR
					{
						UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

						#ifdef SOFTPARTICLES_ON
							float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
							float partZ = i.projPos.z;
							float fade = saturate (_InvFade * (sceneZ-partZ));
							i.color.a *= fade;
						#endif

						half2 normal = UnpackNormal(UNITY_SAMPLE_SCREENSPACE_TEXTURE( _NormalMap, i.uvnormal )).rg;
						half2 distortValue = normal * _DistortFactor * _GrabTextureMobile_TexelSize.xy * 10;
						i.uvgrab.xy = (distortValue * i.uvgrab.z) + i.uvgrab.xy;

						half4 distort = tex2Dproj( _GrabTextureMobile, UNITY_PROJ_COORD(i.uvgrab));
						half4 mask = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_Mask,i.uvmask);
						
						half4 res = distort;
						res.a = _TintColor.a * i.color.a * mask.a;
						return res;
					}
				ENDCG
			}
		}
	}
}