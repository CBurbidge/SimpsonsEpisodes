open SimpsonsEpisodes.Parser
open SimpsonsEpisodes.Plotter
open System

[<EntryPoint>]
let main argv = 
    let episodes = SimpsonsEpisodes.Parser.getEpisodes
    
    let results = SimpsonsEpisodes.Analyser.analyseEpisodes episodes
//    let seasonOne = SimpsonsEpisodes.Analyser.SimpsonsSeason(1, [SimpsonsEpisodes.Analyser.SimpsonsEpisode(1, 1, "a ", "bart", "")])
//    let seasonTwo = SimpsonsEpisodes.Analyser.SimpsonsSeason(2, [SimpsonsEpisodes.Analyser.SimpsonsEpisode(2, 1, "a ", "bart", "bart")])
//    let results = SimpsonsEpisodes.Analyser.SimpsonsFilmography([seasonOne; seasonTwo])
    SimpsonsEpisodes.Plotter.plot results |> ignore
    
    Console.WriteLine("")
    Console.WriteLine("Finished stuff")
    Console.ReadKey() |> ignore
    0 // return an integer exit code