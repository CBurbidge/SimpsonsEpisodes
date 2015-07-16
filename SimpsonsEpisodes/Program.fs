open SimpsonsEpisodes.Parser
open System

[<EntryPoint>]
let main argv = 
    
    let episodes = SimpsonsEpisodes.Parser.getEpisodes
    let results = SimpsonsEpisodes.Analyser.analyseEpisodes episodes
    
    Console.WriteLine("")
    Console.WriteLine("Finished stuff")
    Console.ReadKey() |> ignore
    0 // return an integer exit code