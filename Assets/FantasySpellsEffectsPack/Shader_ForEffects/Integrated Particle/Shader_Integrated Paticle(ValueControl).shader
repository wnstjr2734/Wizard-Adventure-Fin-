﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GAPH Custom Shader/Integrated Particle/Integrated Particle (ValueControl)"{
	Properties{
		[HDR]_TintColor("Tint Color",Color) = (0.5,0.5,0.5,0.5)
		_AllColorFactor("All Color Factor",float) = 1.0
		_ColorRedFactor("Color Red Factor", float) = 1.0
		_ColorGreenFactor("Color Green Factor", float) = 1.0
		_ColorBlueFactor("Color Blue Factor",float) = 1.0
		_MainTex("Particle Texture", 2D) = "white" {}
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc ("BlendSrc", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendDst ("BlendDst", float) = 1
		[Toggle(IS_TEXTUREBLEND)]_TextureBlend("Is Texture Blend",int) = 0
		_InvFade("Soft Particle Factor", Range(0.01,3.0)) = 1.0
	}
		Category{
				Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
				Blend [_BlendSrc] [_BlendDst]
				ColorMask RGB
				Cull Off
				Lighting Off
				ZWrite Off

				SubShader {
					Pass{
						CGPROGRAM
						#pragma vertex vert
						#pragma fragment frag
						#pragma target 3.0
						#pragma multi_compile_instancing
						#pragma multi_compile_particles
						#pragma multi_compile_fog
						#pragma shader_feature IS_TEXTUREBLEND

						#include "UnityCG.cginc"

						UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);

						half4 _TintColor;
                        
						half _AllColorFactor;
						half _ColorRedFactor;
						half _ColorGreenFactor;
						half _ColorBlueFactor;

						struct appdata_t {
							half4 vertex : POSITION;
							half4 color : COLOR;
							#ifdef IS_TEXTUREBLEND
								half4 texcoord : TEXCOORD0;
								half texcoordBlend : TEXCOORD1;
							#else
								half2 texcoord : TEXCOORD0;
							#endif
					
							UNITY_VERTEX_INPUT_INSTANCE_ID //Insert	
						};

						struct v2f {
							half4 vertex : SV_POSITION;
							half4 color : COLOR;
							half2 texcoord : TEXCOORD0;
							#ifdef IS_TEXTUREBLEND
								half3 texcoord2 : TEXCOORD1;
							#endif
							UNITY_FOG_COORDS(2)
							#ifdef SOFTPARTICLES_ON
								half4 projPos : TEXCOORD3;
							#endif

							UNITY_VERTEX_OUTPUT_STEREO //Insert
						};

						half4 _MainTex_ST;

						v2f vert(appdata_t v)
						{
							v2f o;
							
							UNITY_SETUP_INSTANCE_ID(v); //Insert
							UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
							UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

							o.vertex = UnityObjectToClipPos(v.vertex);
							
							#ifdef SOFTPARTICLES_ON
								o.projPos = ComputeScreenPos(o.vertex);
								COMPUTE_EYEDEPTH(o.projPos.z);
							#endif
							o.color = v.color;
							#ifdef IS_TEXTUREBLEND
								o.texcoord = v.texcoord.xy;
								o.texcoord2.xy = v.texcoord.zw;
								o.texcoord2.z = v.texcoordBlend;
							#else
								o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
							#endif
							UNITY_TRANSFER_FOG(o, o.vertex);
							return o;
						}
						
						UNITY_DECLARE_SCREENSPACE_SHADOWMAP(_CameraDepthTexture);
						half _InvFade;

						half4 frag(v2f i ) : SV_Target
						{
							UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

                            #ifdef SOFTPARTICLES_ON
								half sceneZ = LinearEyeDepth(UNITY_SAMPLE_SHADOW_PROJ(UNITY_SAMPLE_SCREEN_SHADOW(_CameraDepthTexture, i.projPos)));
								half partZ = i.projPos.z;
								half fade = saturate(_InvFade * (sceneZ - partZ));
                                i.color.a *= fade;
                            #endif

							half4 tex = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex,i.texcoord);
							#ifdef IS_TEXTUREBLEND
								half4 tex2 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.texcoord2.xy);
								tex = lerp(tex, tex2, i.texcoord2.z);
							#endif

							tex = float4( //Compose each color data using color factors
                                (tex.r) * _ColorRedFactor * 0.75f,
                                (tex.g) * _ColorGreenFactor * 0.75f,
                                (tex.b) * _ColorBlueFactor* 0.75f, tex.a);

							half4 res = _AllColorFactor * i.color * _TintColor * tex; //Merge all factor
							UNITY_APPLY_FOG_COLOR(i.fogCoord, res, half4(0,0,0,0));
							return res;
						}
						ENDCG
				}
			}
	}
}