using System;

namespace Size.Api.Helpers
{
    /// <summary>
    /// Injection Helper.
    /// Classe auxiliar para a injeção de dependeência.
    /// </summary>
    public static class InjectionHelper
    {
        /// <summary>
        /// Verifica se o tipo genérico é compatível com o tipo base
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
