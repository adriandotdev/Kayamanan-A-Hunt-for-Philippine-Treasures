-> main
VAR addedWord = ""

=== main ===
    Tungkol saan {addedWord} ang gusto mo malaman? #name:Aling Suzy
        + [National Heroes]
            Sinong national heroes ang gusto mo malaman?
            + + [Jose Rizal]
                Si <color=\#331313>Jose Rizal</color> ay ang ating pambansang bayani na ipinananganak sa <color=\#331313>Calamba, Laguna</color> noong June 19, 1861. 
                + + + [Next]
                ~ addedWord = "pa"
                -> another_question
            + + [Andres Bonifacio]
                Si <color=\#331313>Andres Bonifacio</color> ay ang <color=\#331313>Ama ng Katipunan</color>.
                + + + [Next]
                ~ addedWord = "pa"
                -> another_question
        + [Tourist Attractions]
            Anong lugar sa <color=\#331313>Region 1</color> ang gusto mong malaman?
            + + [Luneta Park]
                Ang <color=\#331313>Luneta Park</color> o dating tawag na <color=\#331313>Bagumbayan</color> ay ang lugar kung saan pinatay ang ating pambansang bayani na si <color=\#331313>Jose Rizal</color>.
                + + + [Next]
                ~ addedWord = "pa"
                -> another_question
            + + [Mayon Volcano]
                Ang <color=\#331313>Mayon Volcano</color> ay isang tanyag at aktibong bulkan na matatagpuan sa <color=\#331313>Albay</color>
                + + + [Next]
                ~ addedWord = "pa"
                -> another_question
        -> DONE
-> END

=== another_question ===
    May gusto ka {addedWord} bang itanong?
    + [Meron pa]
    -> main
    + [Wala na]
-> END
