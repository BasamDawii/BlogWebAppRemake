using Dapper;
using Newtonsoft.Json;
using Npgsql;

namespace BlogWebAppTests;

public static class Helper
{
    public static readonly Uri Uri;
    public static readonly string ProperlyFormattedConnectionString;
    public static readonly NpgsqlDataSource DataSource;

    static Helper()
    {
        string rawConnectionString;
        string envVarKeyName = "pgconn";

        rawConnectionString = Environment.GetEnvironmentVariable(envVarKeyName)!;
        if (rawConnectionString == null)
        {
            throw new Exception("Connection string environment variable is empty or null.");
        }

        try
        {
            Uri = new Uri(rawConnectionString);
            ProperlyFormattedConnectionString = string.Format(
                "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
                Uri.Host,
                Uri.AbsolutePath.Trim('/'),
                Uri.UserInfo.Split(':')[0],
                Uri.UserInfo.Split(':')[1],
                Uri.Port > 0 ? Uri.Port : 5432);
            DataSource =
                new NpgsqlDataSourceBuilder(ProperlyFormattedConnectionString).Build();
            DataSource.OpenConnection().Close();
        }
        catch (Exception e)
        {
            throw new Exception("The provided connection string is invalid.", e);
        }
    }
    

    public static string BadResponseBody(string content)
    {
        return $@"
Tried converting the response body from the API into a class object, but something went wrong. Below are the details:

RESPONSE BODY: {{content}}

EXCEPTION:
";
    }

    public static void TriggerRebuild()
    {
        using (var conn = DataSource.OpenConnection())
        {
            try
            {
                conn.Execute(RebuildScript);
            }
            catch (Exception e)
            {
                throw new Exception("THERE WAS AN ERROR REBUILDING THE DATABASE.", e);
            }
        }
    }

    public static string MyBecause(object actual, object expected)
    {
        string expectedJson = JsonConvert.SerializeObject(expected, Formatting.Indented);
        string actualJson = JsonConvert.SerializeObject(actual, Formatting.Indented);

        return $"because we want these objects to be equivalent:\nExpected:\n{expectedJson}\nActual:\n{actualJson}";
    }

    public static string RebuildScript = @"
-- Drop the schema if it exists
DROP SCHEMA IF EXISTS blog_schema_test CASCADE;

-- Create a new schema
CREATE SCHEMA blog_schema_test;

-- Create User table
CREATE TABLE IF NOT EXISTS blog_schema_test.users (
    id SERIAL PRIMARY KEY,
    full_name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    role VARCHAR(15) NOT NULL DEFAULT 'User'
);


CREATE TABLE IF NOT EXISTS blog_schema_test.password_hash (
    user_id INTEGER,
    hash VARCHAR(350) NOT NULL,
    salt VARCHAR(180) NOT NULL,
    algorithm VARCHAR(12) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES blog_schema_test.users (id) ON DELETE CASCADE
);

-- Create Category table
create table if not exists blog_schema_test.categories (
    id serial primary key,
    name varchar(255) not null
);

-- Insert known categories for blogs
INSERT INTO blog_schema_test.categories (name) VALUES ('Technology');
INSERT INTO blog_schema_test.categories (name) VALUES ('Health');
INSERT INTO blog_schema_test.categories (name) VALUES ('Travel');
INSERT INTO blog_schema_test.categories (name) VALUES ('Food');
INSERT INTO blog_schema_test.categories (name) VALUES ('Fashion');
INSERT INTO blog_schema_test.categories (name) VALUES ('Lifestyle');
INSERT INTO blog_schema_test.categories (name) VALUES ('Sports');
INSERT INTO blog_schema_test.categories (name) VALUES ('Finance');
INSERT INTO blog_schema_test.categories (name) VALUES ('Politics');
INSERT INTO blog_schema_test.categories (name) VALUES ('Entertainment');

-- Create Blog table
create table if not exists blog_schema_test.blogs (
    id serial primary key,
    user_id int not null,
    title varchar(255) not null,
    content text,
    publication_date timestamp not null,
    category_id int not null,
    featured_image varchar(255),
    constraint fk_user foreign key (user_id) references blog_schema_test.users (id) on delete cascade,
    constraint fk_category foreign key (category_id) references blog_schema_test.categories (id) on delete cascade
);

-- Create Comment table
create table if not exists blog_schema_test.comments (
    id serial primary key,
    blog_id int not null,
    user_id int not null,
    text text not null,
    publication_date timestamp not null,
    constraint fk_blog_comment foreign key (blog_id) references blog_schema_test.blogs (id) on delete cascade,
    constraint fk_user_comment foreign key (user_id) references blog_schema_test.users (id) on delete cascade
);

-- Insert an initial user
INSERT INTO blog_schema_test.users (full_name, email, role)
VALUES ('Blog Team', 'blog-team@example.com', 'Admin');
INSERT INTO blog_schema_test.password_hash (user_id, hash, salt, algorithm)
VALUES (1, 'kQUtcsQ7l1ejwHcL3DUvp8uE0479p4QCF5GOXOGqsLUg0f2daYSv6ot7LsKu2CtsDTAxVyDq0eA1WPSb3lLwqLEwJnLwd1fINC0RH9lcx/vOJskoSimj5oD3brvHRBFQwDAv21rCQwIzYgMNMI0kmW7xAj8GdyHHUQm9pGWue/q1uhNBre4b+aeieOCc7/phA6uJols+I+oI3AR4Mu2+KYJvNbOTIYu4SGVR15Gvnz+eeEAYpQF7KvVVYc+E+9PnDdk/nDWLDpovq8QO/KWtDqqRokCLC9t5shhsSPlHxaQkmcRSSHCbXf0gZTvl0HiSNVgd8RdyiGJH1EvDwbN82A==', 'hMadBxcqW79tqCgxSjfHAKdzOdhxkzAyU2gXflHT2FohnX8Vvq50690TMNrfrJ56C0wb3J3LR+92uCABJuBzit95dVaT8NpeWuc8WdasMHFvu3XHZxSFCNn1PZu9Vf3t0+KWAKyqW7+7udVfwBzqE/OkZBbhNd/EB9FW3VrDOB4=', 'argon2id');

-- Insert some initial blogs
INSERT INTO blog_schema_test.blogs (user_id, title, content, publication_date, category_id, featured_image)
VALUES 
(1, 'Tech Blog Test', 'This is the first test blog post.', '2024-01-10 10:00:00', 1, 'https://business.leeds.ac.uk/images/Disrupting_Tech_shutterstock_566877226_800_x_400.jpg');

INSERT INTO blog_schema_test.blogs (user_id, title, content, publication_date, category_id, featured_image)
VALUES 
(1, 'Health Blog Test', 'This is the second test blog post.', '2024-01-11 10:00:00', 2, 'https://www.statnews.com/wp-content/uploads/2022/03/AdobeStock_246942922.jpeg');

INSERT INTO blog_schema_test.blogs (user_id, title, content, publication_date, category_id, featured_image)
VALUES 
(1, 'Travel Blog Test', 'This is the third test blog post.', '2024-01-12 10:00:00', 3, 'https://www.candorblog.com/wp-content/uploads/2017/05/travel-022.jpg');
";

    public static string NoResponseMessage = "Failed to get a response from the API. Please check your request and try again.";
}