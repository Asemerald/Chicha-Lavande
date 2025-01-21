Shader "Custom/InvertColorWithColorOverLifetime"
{
    Properties {
        _TintColor ("Tint Color", Color) = (0,0,0,1) // Teinte globale avec alpha
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Pass
        {
            Blend OneMinusDstColor OneMinusSrcColor, SrcAlpha OneMinusSrcAlpha // Mélange pour inversion et transparence
            ColorMask RGBA                          // Prend en compte alpha et couleur

            ZWrite Off                              // Pas d'écriture dans le tampon de profondeur
            ZTest LEqual                            // Respecte les objets derrière

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _TintColor; // Propriété globale de teinte

            struct appdata_t {
                float4 vertex : POSITION; // Position des sommets
                float4 color : COLOR;     // Couleur des sommets, fournie par Unity (Color over Lifetime)
            };

            struct v2f {
                float4 pos : SV_Position; // Position projetée
                fixed4 color : COLOR;     // Couleur interpolée
            };

            // Vertex shader : transmet la position et la couleur
            v2f vert(appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Transformation en coordonnées écran
                o.color = v.color; // Passe la couleur du sommet, incluant l'alpha
                return o;
            }

            // Fragment shader : applique l'inversion et respecte l'alpha
            fixed4 frag(v2f i) : SV_Target {
                // Multiplie la couleur des particules avec la teinte globale
                fixed4 finalColor = i.color * _TintColor;

                // Inversion des couleurs (uniquement RGB)
                finalColor.rgb = 1.0 - finalColor.rgb;

                // Retourne la couleur inversée avec l'alpha respecté
                return finalColor;
            }

            ENDCG
        }
    }
}
