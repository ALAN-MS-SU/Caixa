using Microsoft.AspNetCore.Mvc;
using CaixaAPI.DB;
using CaixaAPI.Model.Playlist;
using Microsoft.EntityFrameworkCore;

namespace CaixaAPI.Controllers;

[ApiController]
[Route("/Playlist")]
public class PlaylistController(Context context) : ControllerBase
{
    private Context Context = context;

    [HttpGet("{ID}")]
    public async Task<IActionResult> GetPlaylist(int ID)
    {
        var FullList = await Context.Playlists.Where(playlist =>  playlist.PlaylistID == ID)
            .Select(playlist => new{ID = playlist.ID, PlaylistID = playlist.PlaylistID, Name = playlist.Name, User = playlist.User.Name, Link = playlist.Link})
            .ToListAsync();
        var Playlist = new
        {
            ID = FullList[0].PlaylistID, Name = FullList[0].Name, User = FullList[0].User,
            Items = FullList.Select(playlist => new {ID = playlist.ID,Link = playlist.Link})
        };
        return Ok(Playlist);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var Playlists = await Context.Playlists.GroupBy(playlist => playlist.PlaylistID)
            .Select(playlist => new {ID = playlist.Key, Name = playlist.First().Name, User = playlist.First().User.Name}).ToListAsync();
            
        return Ok(Playlists);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreatePlayList playlist)
    {
        var user = await Context.Users.FindAsync(playlist.User);
        if (user == null)
        {
            return Unauthorized("Usuaŕio não existe");
        }
        var ID = Context.Playlists.Select(p=>(int?)p.PlaylistID).Max()??1;
        Console.WriteLine(ID.ToString());
        await Context.Playlists.AddAsync(new Playlist{PlaylistID = ID,Name = playlist.Name, UserID = playlist.User, Link = playlist.Link});
        int row = await Context.SaveChangesAsync();
        if (row > 0)
        {
            return StatusCode(201,"Playlist foi Criada");
        }
        return BadRequest("Erro ao criar playlist");
    }
}