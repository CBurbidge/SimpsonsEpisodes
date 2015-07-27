module SimpsonsEpisodes.Plotter
//
//open SimpsonsEpisodes.Analyser
//open RDotNet
//open RProvider
//open RProvider.graphics
//
//let getRatios(filmography:SimpsonsFilmography, getResult) =
//    filmography.seasons
//    |> List.map getResult
//    |> List.map (fun x -> (double)x)
//
//let plotLine(seasonNumbers, values, colour:string) =
//    namedParams [
//        "x", box seasonNumbers;
//        "y", box values;
//        "type", box "o";
//        "col", box colour;
//    ]
//    |> R.lines
//    |> ignore
//
//let plot (filmography: SimpsonsFilmography) =
//    
//    let seasonNumbers = filmography.seasons
//                        |> List.map (fun x -> x.seasonNumber)
//
//    let maggieRatios = getRatios(filmography, (fun x -> x.result.Maggie.frequency))
//    let lisaRatios = getRatios(filmography, (fun x -> x.result.Lisa.frequency))
//    let bartRatios = getRatios(filmography, (fun x -> x.result.Bart.frequency))
//    let margeRatios = getRatios(filmography, (fun x -> x.result.Marge.frequency))
//    let homerRatios = getRatios(filmography, (fun x -> x.result.Homer.frequency))
//
//    let biggestValue: double = List.concat ([maggieRatios; lisaRatios; bartRatios; margeRatios; homerRatios ])
//                               |> List.max
//    
//    namedParams [   
//        "x", box seasonNumbers; 
//        "ylim", box [0.0; biggestValue] 
//        ]
//    |> R.plot
//    |> ignore
//    
//    plotLine(seasonNumbers, maggieRatios, "red")
//    plotLine(seasonNumbers, lisaRatios, "blue")
//    plotLine(seasonNumbers, bartRatios, "green")
//    plotLine(seasonNumbers, margeRatios, "yellow")
//    plotLine(seasonNumbers, homerRatios, "black")
//    0