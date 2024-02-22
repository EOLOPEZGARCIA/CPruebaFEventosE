// El siguiente bloque ifdef muestra la forma estándar de crear macros que facilitan 
// desde un DLL más sencillo. Todos los archivos de este archivo DLL se compilan con LIBRERIAELSTER_EXPORTS
// definido en la línea de comandos. Este símbolo no debe definirse en ningún proyecto
// que use este archivo DLL. De este modo, otros proyectos cuyos archivos de código fuente incluyan el archivo 
// las funciones de LIBRERIAELSTER_API se importan de un archivo DLL, mientras que este archivo DLL ve símbolos
// definidos en esta macro como si fueran exportados.
#ifdef LIBRERIAELSTER_EXPORTS
#define LIBRERIAELSTER_API __declspec(dllexport)
#else
#define LIBRERIAELSTER_API __declspec(dllimport)
#endif

// Esta clase se exporta desde LibreriaElster.dll
class LIBRERIAELSTER_API CLibreriaElster {
public:
	CLibreriaElster(void);
	// TODO: agregar métodos aquí.
};

extern LIBRERIAELSTER_API int nLibreriaElster;

LIBRERIAELSTER_API int fnLibreriaElster(void);
