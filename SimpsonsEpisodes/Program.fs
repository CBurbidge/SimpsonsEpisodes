open SimpsonsEpisodes
open System

[<EntryPoint>]
let main argv = 
    
    SimpsonsEpisodes.downloadSeasonFilesToDisk
    
    let allEpisodes: EpisodeSummaryInfo list = SimpsonsEpisodes.getAllEpisodes()
    
    SimpsonsEpisodes.ensureThatEpisodeFilesExist allEpisodes
    
    Console.ReadKey() |> ignore
    0 // return an integer exit code