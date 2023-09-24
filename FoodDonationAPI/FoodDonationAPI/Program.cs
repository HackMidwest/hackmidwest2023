using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

string connectionString = app.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;

try
{
    //using var conn = new SqlConnection(connectionString);
    //conn.Open();

    //var command = new SqlCommand(
    //    "CREATE TABLE Post (ID int NOT NULL PRIMARY KEY IDENTITY, Time datetime, Author varchar(255), Type varchar(255), Text varchar(255), ProfilePicture varchar(255), Image varchar(255), ZipCode int, Claimant int);" +
    //    "CREATE TABLE Profile (ID int NOT NULL PRIMARY KEY IDENTITY, Name varchar(255), Password varchar(255), PictureUrl varchar(255));"
    //    conn);
    //using SqlDataReader reader = command.ExecuteReader();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

app.MapGet("/Posts", () => {
    var rows = new List<string>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand("SELECT * FROM Post", conn);
    using SqlDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add($"{reader.GetInt32(0)}, {reader.GetDateTime(1)}, {reader.GetString(2)},{reader.GetString(3)}, {reader.GetString(4)}, {reader.GetInt32(5)}, {reader.GetInt32(6)}");
        }
    }

    return rows;
})
.WithName("GetPosts")
.WithOpenApi();

app.MapGet("/UnclaimedPosts", () => {
    var rows = new List<UnclaimedPostModel>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand("SELECT po.Time, po.Author, po.Type, po.Text, po.ZipCode, pr.PictureUrl, ft.PictureUrl FROM Post po left join Profile pr on po.Author = pr.name left join FoodType ft on po.Type = ft.Name where Claimant = -1", conn);
    using SqlDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add(
                new UnclaimedPostModel()
                {
                    Time = reader.GetDateTime(0),
                    Author = reader.GetString(1),
                    Type = reader.GetString(2),
                    Text = reader.GetString(3),
                    ZipCode = reader.GetInt32(4),
                    ProfilePictureUrl = reader.GetString(5),
                    FoodTypePictureUrl = reader.GetString(6)
                }
            );
        }
    }

    return rows;
})
.WithName("UnclaimedPosts")
.WithOpenApi();

app.MapGet("/ClaimedPosts/{id}", (int id) => {
    var rows = new List<UnclaimedPostModel>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand("SELECT po.Time, po.Author, po.Type, po.Text, po.ZipCode, pr.PictureUrl, ft.PictureUrl FROM Post po left join Profile pr on po.Author = pr.name left join FoodType ft on po.Type = ft.Name where Claimant = @id", conn);
    command.Parameters.Clear();
    command.Parameters.AddWithValue("@id", id);

    using SqlDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add(
                new UnclaimedPostModel()
                {
                    Time = reader.GetDateTime(0),
                    Author = reader.GetString(1),
                    Type = reader.GetString(2),
                    Text = reader.GetString(3),
                    ZipCode = reader.GetInt32(4),
                    ProfilePictureUrl = reader.GetString(5),
                    FoodTypePictureUrl = reader.GetString(6)
                }
            );
        }
    }

    return rows;
})
.WithName("ClaimedPosts")
.WithOpenApi();

app.MapGet("/Posts/{author}", (string author) => {
    var rows = new List<string>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand("SELECT * FROM Post WHERE Author = @author", conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@author", author);

    using SqlDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add($"{reader.GetInt32(0)}, {reader.GetDateTime(1)}, {reader.GetString(2)}, {reader.GetString(3)},{reader.GetString(4)}, {reader.GetString(5)}, {reader.GetString(6)}, {reader.GetInt32(7)}, {reader.GetInt32(8)}");
        }
    }

    return rows;
})
.WithName("GetAuthorsPosts")
.WithOpenApi();

app.MapPost("/CreatePost", (PostModel post) => {
    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
        "INSERT INTO Post (Time, Author, Type, Text, Zipcode, Claimant) VALUES (@time, @author, @type, @text, @zipCode, @claimant)",
        conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@time", post.Time);
    command.Parameters.AddWithValue("@author", post.Author);
    command.Parameters.AddWithValue("@type", post.Type);
    command.Parameters.AddWithValue("@text", post.Text);
    command.Parameters.AddWithValue("@zipCode", post.ZipCode);
    command.Parameters.AddWithValue("@claimant", post.Claimant);

    using SqlDataReader reader = command.ExecuteReader();
})
.WithName("CreatePost")
.WithOpenApi();

app.MapDelete("/Post/delete/{id}", (int id) => {
    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
        "DELETE FROM Post WHERE Id = @id",
        conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@id", id);

    using SqlDataReader reader = command.ExecuteReader();
})
.WithName("DeletePost")
.WithOpenApi();

app.MapPost("/UpdatePost", (GetModel post) => {
    var rows = new List<string>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
         "Update Post Set Claimant = @Claimant Where ID = @Id;",
         conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@Claimant", post.Claimant);
    command.Parameters.AddWithValue("@Id", post.Id);

    using SqlDataReader reader = command.ExecuteReader();
})
.WithName("UpdatePost")
.WithOpenApi();

app.MapGet("/Profiles", () => {
    var rows = new List<string>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand("SELECT * FROM Profile", conn);
    using SqlDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add($"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetString(3)}");
        }
    }

    return rows;
})
.WithName("GetProfiles")
.WithOpenApi();


app.MapPost("/CreateProfile", (PostProfile profile) => {
    var rows = new List<string>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
         "INSERT INTO Profile (Name, Password, PictureUrl) VALUES (@name, @password, @pictureurl)",
         conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@name", profile.Name);
    command.Parameters.AddWithValue("@password", profile.Password);
    command.Parameters.AddWithValue("@pictureurl", profile.PictureUrl);

    using SqlDataReader reader = command.ExecuteReader();
})
.WithName("PostProfile")
.WithOpenApi();

app.MapPost("/Login", (LoginProfile profile) => {
    var getProfile = new GetProfile();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
         "SELECT ID, Name, PictureUrl FROM Profile WHERE Name = @name and Password = @password",
         conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@name", profile.Name);
    command.Parameters.AddWithValue("@password", profile.Password);

    using SqlDataReader reader = command.ExecuteReader();
    if (reader.HasRows)
    {
        while (reader.Read())
        {
            getProfile.Id = reader.GetInt32(0);
            getProfile.Name = reader.GetString(1);
            getProfile.PictureUrl = reader.GetString(2);

        }
    }
    return getProfile;
})
.WithName("Login")
.WithOpenApi();

app.Run();

public class PostModel
{
    public DateTime Time { get; set; }
    public string Author { get; set; }
    public string Type { get; set; }
    public string Text { get; set; }
    public int ZipCode { get; set; }
    public int Claimant { get; set; }
}

public class UnclaimedPostModel
{
    public DateTime Time { get; set; }
    public string Author { get; set; }
    public string Type { get; set; }
    public string Text { get; set; }
    public int ZipCode { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string FoodTypePictureUrl { get; set; }
}

public class GetModel
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public string Author { get; set; }
    public string Type { get; set; }
    public string Text { get; set; }
    public int ZipCode { get; set; }
    public int? Claimant { get; set; }
}

public class GetProfile
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
}

public class LoginProfile
{
    public string Name { get; set; }
    public string Password { get; set; }
}
public class PostProfile
{
    public string Name { get; set; }
    public string? Password { get; set; }
    public string? PictureUrl { get; set; }
}