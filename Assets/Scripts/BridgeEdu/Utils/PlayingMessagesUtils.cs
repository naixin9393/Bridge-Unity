using System.Collections.Generic;
using BridgeEdu.Core;

namespace BridgeEdu.Utils {
    public static class PlayingMessagesUtils {
        public const string Unknown = "Situación desconocida";

        public static string OpeningHonorSequence(List<Card> honorSequence) {
            if (honorSequence.Count == 0) return "No hay secuencia de honores";
            string sequence = string.Join(", ", honorSequence);
            return $"Secuencia de honores: {sequence}, jugar la mayor de la secuencia";
        }

        public static string OpeningFourthHighest(List<Card> suitWithHonor) {
            if (suitWithHonor.Count < 4) return "No hay suficientes cartas en el palo para jugar la cuarta más alta";
            string cards = string.Join(", ", suitWithHonor);
            return $"Cuarta del palo largo: {cards}, jugar la cuarta más alta";
        }
    }
}