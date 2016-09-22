#ifdef _OSX

#define TECHNIQUE(name, vsname, psname ) \
	technique name { pass { VertexShader = compile vs_3_0 vsname (); PixelShader = compile ps_3_0 psname(); } }
	
#define TECHNIQUE_PSONLY(name, psname ) \
	technique name { pass { PixelShader = compile ps_3_0 psname(); } }
	
#else

#define TECHNIQUE(name, vsname, psname ) \
	technique name { pass { VertexShader = compile vs_4_0 vsname (); PixelShader = compile ps_4_0 psname(); } }
	
#define TECHNIQUE_PSONLY(name, psname ) \
	technique name { pass { PixelShader = compile ps_4_0 psname(); } }

#endif