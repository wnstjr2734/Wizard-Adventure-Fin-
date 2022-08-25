﻿Shader "GAPH Custom Shader/Dissolve Texture/Mobile/Mobile - Dissolve Texture(TextureAnimation)"{
	Properties{
		[HDR]_TintColor("Tint Color",Color) = (0,0,0,0)
        _CutOut("CutOut",Range(0,1)) = 1
		_MainTex("Main Texture",2D) = "white"{}
		_NormalTex("Normal Texture", 2D) = "black"{}
		_Mask("Mask",2D) = "white"{}
		_Speed("Offset Speed",Float) = 1
        _ColorStrength("Color Strength",Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc("BlendSrc", float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_BlendDst("BlendDst", float) = 1
	}

		Category{

			Tags{ "Queue" = "Transparent"  "IgnoreProjector" = "True"  "RenderType" = "Transparent" }
			Blend [_BlendSrc] [_BlendDst]
			Cull Off
			Lighting Off
			ZWrite Off

		SubShader{
			GrabPass{
			Name "BASE"
			Tags{ "LightMode" = "Always" }
			}
				Pass{
				Name "BASE"

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
                #pragma multi_compile_instancing

				#include "UnityCG.cginc"

				UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_Mask);
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_NormalTex);

				half4 _TintColor;
                half _CutOut;
                half _Speed;
                half _ColorStrength;


				struct appdata_t {
					float4 vertex : POSITION;
					half4 color : COLOR;
					float2 texcoord : TEXCOORD0;
                    float2 texcoord2 : TEXCOORD1;
					
					UNITY_VERTEX_INPUT_INSTANCE_ID //Insert	
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half4 color : COLOR;
					float2 texcoord : TEXCOORD0;
                    float2 texcoord2 : TEXCOORD1;
					float2 uvmask : TEXCOORD2;
					UNITY_FOG_COORDS(2)
					
					UNITY_VERTEX_OUTPUT_STEREO //Insert
				};

				half4 _MainTex_ST;
				half4 _Mask_ST;
				half4 _NormalTex_ST;

				v2f vert(appdata_t v)
				{
					v2f o;

					UNITY_SETUP_INSTANCE_ID(v); //Insert
					UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

					o.vertex = UnityObjectToClipPos(v.vertex);

					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.texcoord2 = TRANSFORM_TEX(v.texcoord, _NormalTex);
					o.uvmask = TRANSFORM_TEX(o.texcoord, _Mask);

					UNITY_TRANSFER_FOG(o, o.vertex);
					return o;
				}

				half4 frag(v2f i) : Color
				{
                    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

					half2 distort = UnpackNormal(UNITY_SAMPLE_SCREENSPACE_TEXTURE(_NormalTex, i.texcoord2)).rg;
					half4 tex = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.texcoord.xy + distort.xy / 10 - (_Speed * _Time / 10));
                    tex = pow(tex,3);

                    half4 mask = saturate(UNITY_SAMPLE_SCREENSPACE_TEXTURE(_Mask,i.uvmask));

                    half4 res = _ColorStrength * i.color * _TintColor * tex;
                    half alpha = res * mask.a * (_ColorStrength * 10);
                    res.a = saturate(pow(alpha, 1.25))-_CutOut; //Just change alpha value for low calculate rate with cutout value

					UNITY_APPLY_FOG(i.fogCoord, col);
					return res;
				}
				ENDCG
			}
		}
	}
}