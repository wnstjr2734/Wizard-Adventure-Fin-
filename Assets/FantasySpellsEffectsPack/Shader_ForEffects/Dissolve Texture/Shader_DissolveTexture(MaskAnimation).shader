﻿Shader "GAPH Custom Shader/Dissolve Texture/Dissolve Texture(MaskAnimation)"{
	Properties{
		[HDR]_TintColor("Tint Color",Color) = (0,0,0,0)
        _CutOut("CutOut",Range(0,1)) = 1
		_MainTex("Main Texture",2D) = "white"{}
		_NormalTex("Normal Texture", 2D) = "black"{}
		_Mask("Mask",2D) = "white"{}
		_Speed("Offset Speed",Float) = 1
        _ColorStrength("Color Strength",Float) = 1
		_DistortFactor("Distort Factor",Float) = 1
		_DistortTimeFactor("Distort Time factor",Float) = 1
		[MaterialToggle] _isBlendAdd("isBlendAdd", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc("BlendSrc", float) = 1 //Blend mode start
		[Enum(UnityEngine.Rendering.BlendMode)]_BlendDst("BlendDst", float) = 1 //Blend mode end
	}

		Category{

			Tags{ "Queue" = "Transparent"  "IgnoreProjector" = "True"  "RenderType" = "Transparent" }
			Blend [_BlendSrc] [_BlendDst]
			Cull Off
			Lighting Off
			ZWrite Off

		SubShader{
				Pass{

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_Mask);
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_NormalTex);

				half4 _TintColor;
                half _CutOut;
                half _Speed;
                half _ColorStrength;
				half _DistortFactor;
				half _DistortTimeFactor;
				half _isBlendAdd;

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
					UNITY_FOG_COORDS(3)
					
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

					half2 distort = UnpackNormal(UNITY_SAMPLE_SCREENSPACE_TEXTURE(_NormalTex, i.texcoord2 * 1.0f+(_Speed*_Time.x/20 * _DistortTimeFactor))).rg; // Get distort info from normal texture.
					distort += UnpackNormal(UNITY_SAMPLE_SCREENSPACE_TEXTURE(_NormalTex, i.texcoord2 * 1.0f - (_Speed*_Time.x / 20 * _DistortTimeFactor) + float2(0.5f,0.15f))).rg;

                    half4 tex = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.texcoord.xy + distort.x * _DistortFactor - (_Speed * _Time/20)); //Animate main texture using time and distort value.
                    tex *= UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.texcoord.xy + distort.y * _DistortFactor + (_Speed * _Time/20));
                    
					half4 tex2 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.texcoord.xy + distort.xy * _DistortFactor + (_Speed * _Time/10));
					tex2 *= UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.texcoord.xy + distort.xy * _DistortFactor - (_Speed * _Time / 10));

					half4 tex_all = half4(0, 0, 0, 0);
					if(_isBlendAdd == 0)
						tex_all = (tex * tex2) *_TintColor*7.5f;
					else
						tex_all = (tex + tex2) *_TintColor;

                    half4 mask = saturate(UNITY_SAMPLE_SCREENSPACE_TEXTURE(_Mask,i.uvmask)-_CutOut); // Get mask info with cutout value for cutout texture 

                    half4 res = _ColorStrength * i.color * _TintColor * tex_all; //Compose main r,g,b source using animate tex
                    half alpha = res.a * mask.a * (_ColorStrength*10); //Alpha set using mask
					res.a = saturate(pow(alpha, 1.25));

					UNITY_APPLY_FOG(i.fogCoord, col);
					return res;
				}
				ENDCG
			}
		}
	}
}