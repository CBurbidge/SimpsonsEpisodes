module SimpsonsEpisodes.Plotter

open SimpsonsEpisodes.Analyser

let getRatios(filmography:SimpsonsFilmography, getResult) =
    filmography.seasons
    |> List.map getResult
    |> List.map (fun x -> (double)x)

let plot (filmography: SimpsonsFilmography) =
    
    let seasonNumbers = filmography.seasons
                        |> List.map (fun x -> x.seasonNumber)

    let maggieRatios = getRatios(filmography, (fun x -> x.result.Maggie.frequency))
    let lisaRatios = getRatios(filmography, (fun x -> x.result.Lisa.frequency))
    let bartRatios = getRatios(filmography, (fun x -> x.result.Bart.frequency))
    let margeRatios = getRatios(filmography, (fun x -> x.result.Marge.frequency))
    let homerRatios = getRatios(filmography, (fun x -> x.result.Homer.frequency))

    let biggestValue: double = List.concat ([maggieRatios; lisaRatios; bartRatios; margeRatios; homerRatios ])
                               |> List.max
    
    0