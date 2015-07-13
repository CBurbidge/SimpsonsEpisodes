open SimpsonsEpisodes
open System

[<EntryPoint>]
let main argv = 
    
    SimpsonsEpisodes.downloadSeasonFilesToDisk
    
    let allEpisodesSummaryInfos: EpisodeSummaryInfo list = SimpsonsEpisodes.getAllEpisodes()
    
    SimpsonsEpisodes.ensureThatEpisodeFilesExist allEpisodesSummaryInfos
    
    SimpsonsEpisodes.fixOBrotherEpisode
    
    let episodes = allEpisodesSummaryInfos
                   |> List.map  (fun x -> SimpsonsEpisodes.parseEpisodeFile(x, getEpisodeFileName))
    
    Console.WriteLine("")
    Console.WriteLine("Finished stuff")
    Console.ReadKey() |> ignore
    0 // return an integer exit code