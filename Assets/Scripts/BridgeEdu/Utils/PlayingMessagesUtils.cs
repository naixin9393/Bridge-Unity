using System.Collections.Generic;
using BridgeEdu.Core;

namespace BridgeEdu.Utils {
    public static class PlayingMessagesUtils {
        public const string Unknown = "Situación desconocida";

        public static string OpeningHonorSequence(List<Card> honorSequence) {
            if (honorSequence.Count == 0) return "No hay secuencia de honores";
            string sequence = string.Join(", ", honorSequence);
            return $"Secuencia de honores: {sequence}. Jugar la mayor de la secuencia";
        }

        public static string OpeningFourthHighest(List<Card> suitWithHonor) {
            if (suitWithHonor.Count < 4) return "No hay suficientes cartas en el palo para jugar la cuarta más alta";
            string cards = string.Join(", ", suitWithHonor);
            return $"Cuarta del palo largo: {cards}. Jugar la cuarta más alta";
        }

        public static string OpeningTwoHonorTwoBelow(List<Card> twoBelow) {
            if (twoBelow.Count < 2) return "No hay suficientes cartas para jugar";
            string cards = string.Join(", ", twoBelow);
            return $"Dos honores consecutivos y tercera carta dos por debajo del menor de los honores: {cards}. Jugar el mayor de los honores";
        }

        public static string DiscardLowestCard(List<Card> cards) {
            if (cards.Count == 0) return "No hay cartas para descartar";
            string cardsString = string.Join(", ", cards);
            return $"Descartar cartas. {cardsString}. Descartar la más baja";
        }

        public static string PlayLowestCardConsecutive(List<Card> cards) {
            if (cards.Count == 0) return "No hay cartas para jugar";
            string cardsString = string.Join(", ", cards);
            return $"Cartas equivalentes. {cardsString}. Jugar la más baja";
        }

        public static string DefenderHonorOverHonor = $"Honor sobre honor del atacante. Jugar un honor mayor";
        public static string DefenderSecondTurnPlayLowestCard = "Segunda ronda lado defensor. Jugar la carta más baja si la primera carta no es honor";
    }
}