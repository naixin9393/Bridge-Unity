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

        public static string DefenderHonorOverHonor = $"Segundo turno defensor. Honor sobre honor del atacante. Jugar un honor mayor";
        public static string DefenderSecondTurnPlayLowestCard = "Segundo turno defensor. Jugar la carta más baja si la primera carta no es honor";
        public static string DefenderFourthTurnPlayLowestCardPartnerHighest = "Cuarto turno defensor. Jugar la carta más baja si el compañero ha jugado la carta más alta";
        public static string DefenderFourthTurnPlayLowestCardCantWin = "Cuarto turno defensor. Jugar la carta más baja si no se puede ganar la baza";
        public static string DefenderFourthTurnPlayHigherCard = "Cuarta turno defensor. Si se puede ganar la baza, jugar la carta más pequeña que gane la baza";
        public static string AttackerPlayBiggerCard = $"Segundo turno atacante. Jugar una carta mayor si es posible";
        public static string AttackerFourthTurnPlayLowestCardPartnerHighest = "Cuarto turno atacante. Jugar la carta más baja si la carta del compañero es la más alta";
        public static string AttackerFourthTurnPlayLowestCardCantWin = "Cuarto turno atacante. Jugar la carta más baja si no se puede ganar la baza";
        public static string AttackerFourthTurnPlayHigherCard = "Cuarta turno atacante. Si se puede ganar la baza, jugar la carta más pequeña que gane la baza";
        public static string AttackerFirstTurnPlayLongSuitHighestCard = "Primer turno atacante. Jugar la carta más alta del palo largo si se tiene la carta más alta";
        public static string AttackerFirstTurnPlayLongSuitLowestCard = "Primer turno atacante. Jugar la carta más baja del palo largo si el compañero tiene la carta más alta";
        public static string AttackerSecondTurnPlayLowestCard = "Segundo turno atacante. Jugar la carta más baja si no tiene carta mayor que la del defensor";
        public static string AttackerThirdTurnPlayLongSuitHighestCard = "Tercer turno atacante. Jugar la carta más alta del palo largo si se tiene la carta más alta";
        public static string AttackerThirdTurnPlayLongSuitLowestCard = "Tercer turno atacante. Jugar la carta más baja del palo largo si el compañero tiene la carta más alta";
        public static string AttackerThirdTurnPlayLowestCardPartnerHighest = "Tercer turno atacante. Jugar la carta más baja si la carta del compañero es la más alta";
        public static string AttackerThirdTurnPlayLowestCardCantWin = "Tercer turno atacante. Jugar la carta más baja si no se puede ganar la baza";
        public static string AttackerThirdTurnPlayHigherCard = "Tercer turno atacante. Si se puede ganar la baza, jugar la carta más pequeña que gane la baza";
    }
}