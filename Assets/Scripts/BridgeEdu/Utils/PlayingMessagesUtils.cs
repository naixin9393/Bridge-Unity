using System.Collections.Generic;
using BridgeEdu.Core;

namespace BridgeEdu.Utils {
    public static class PlayingMessagesUtils {
        public const string Unknown = "Situación desconocida";

        public static string HonorSequence(List<Card> honorSequence) {
            if (honorSequence.Count == 0) return "No hay secuencia de honores";
            string sequence = string.Join(", ", honorSequence);
            return $"Secuencia de honores: {sequence}, jugar la mayor de la secuencia";
        }
    }
}