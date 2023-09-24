using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System;

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
    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
        "CREATE TABLE Posts (ID int NOT NULL PRIMARY KEY IDENTITY, Time datetime, Author varchar(255), Text varchar(255), ProfilePicture varchar(255), Image varchar(255), ZipCode int, Claimant int);",
        conn);
    using SqlDataReader reader = command.ExecuteReader();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

app.MapGet("/Posts", () => {
    var rows = new List<string>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand("SELECT * FROM Posts", conn);
    using SqlDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add($"{reader.GetInt32(0)}, {reader.GetDateTime(1)}, {reader.GetString(2)}, {reader.GetString(3)}, {reader.GetString(4)}, {reader.GetString(5)}, {reader.GetInt32(6)}, {reader.GetInt32(7)}");
        }
    }

    return rows;
})
.WithName("GetPosts")
.WithOpenApi();

app.MapGet("/Posts/{author}", (string author) => {
    var rows = new List<string>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand("SELECT * FROM Posts WHERE Author = @author", conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@author", author);

    using SqlDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add($"{reader.GetInt32(0)}, {reader.GetDateTime(1)}, {reader.GetString(2)}, {reader.GetString(3)}, {reader.GetString(4)}, {reader.GetString(5)}, {reader.GetInt32(6)}, {reader.GetInt32(7)}");
        }
    }

    return rows;
})
.WithName("GetAuthorsPosts")
.WithOpenApi();

app.MapPost("/Post", (PostModel post) => {
    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
        "INSERT INTO Posts (Time, Author, Text, ProfilePicture, Image, Zipcode, Claimant) VALUES (@time, @author, @text, @profilePicture, @image, @zipCode, @claimant)",
        conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@time", post.Time);
    command.Parameters.AddWithValue("@author", post.Author);
    command.Parameters.AddWithValue("@text", post.Text);
    command.Parameters.AddWithValue("@profilePicture", post.ProfilePicture);
    command.Parameters.AddWithValue("@image", post.Image);
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
        "DELETE FROM Posts WHERE Id = @id",
        conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@id", id);

    using SqlDataReader reader = command.ExecuteReader();
})
.WithName("DeletePost")
.WithOpenApi();

app.Run();

public class PostModel
{
    public DateTime Time { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public string ProfilePicture { get; set; }
    public string Image { get; set; }
    public int ZipCode { get; set; }
    public int? Claimant { get; set; }
}

public class GetModel
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public string ProfilePicture { get; set; }
    public string Image { get; set; }
    public int ZipCode { get; set; }
    public int? Claimant { get; set; }
}