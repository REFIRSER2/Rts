Shader "Projector/Light" {
  Properties {
  	  _Color ("Main Color", Color) = (1,1,1,1)   	
     _ShadowTex ("Cookie", 2D) = "" { TexGen ObjectLinear }
     _FalloffTex ("FallOff", 2D) = "" { TexGen ObjectLinear }
  }
	Subshader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		Pass {
    		Lighting Off Fog { Mode off } 
			SetTexture [_MainTex] {
				constantColor (0,0,0,1)
				combine constant
			}
		}    
	}
	Subshader 
	{
		Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest"}
		Pass {
    		Lighting Off Fog { Mode off } 
			AlphaTest Greater [_AlphaCutoff]
			Color [_TintColor]
			SetTexture [_MainTex] {
				constantColor (0,0,0,1)
				combine constant, previous * texture
			}
		}    
	}
	Subshader 
	{
		Tags { "RenderType"="TransparentAlphaBlended" "Queue"="Transparent"}
		Pass {
    		Lighting Off Fog { Mode off } 
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Color [_TintColor]
			SetTexture [_MainTex] {
				constantColor (0,0,0,1)
				combine constant, previous * texture
			}
		}    
	}
	Subshader 
	{
		Tags { "RenderType"="TransparentAlphaAdditve" "Queue"="Transparent"}
		Pass {
    		Lighting Off Fog { Mode off } 
			ZWrite Off
			Blend SrcAlpha One
			Color [_TintColor]
			SetTexture [_MainTex] {
				constantColor (0,0,0,1)
				combine constant, previous * texture
			}
		}    
	}
}