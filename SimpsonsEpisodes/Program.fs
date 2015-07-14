open SimpsonsEpisodesParser
open System

[<EntryPoint>]
let main argv = 
    
    let episodes = SimpsonsEpisodesParser.getEpisodes
    
    Console.WriteLine("")
    Console.WriteLine("Finished stuff")
    Console.ReadKey() |> ignore
    0 // return an integer exit code