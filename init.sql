CREATE USER 'admin'@'%' IDENTIFIED WITH 'caching_sha2_password' BY 'admin';
GRANT ALL PRIVILEGES ON workoutnow.* TO 'admin'@'%';
FLUSH PRIVILEGES;

CREATE DATABASE IF NOT EXISTS workoutnow;
USE workoutnow;

CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    surname VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    pass VARCHAR(255) NOT NULL,
    age INT UNSIGNED NOT NULL CHECK (age >= 0),
    workout_name VARCHAR(255) DEFAULT NULL,
    workout_description VARCHAR(255) DEFAULT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS exercises (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) UNIQUE,
    description VARCHAR(255) NOT NULL,
    popularity_score INT DEFAULT 0 NOT NULL,
    last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabella di relazione molti a molti tra users ed exercise
CREATE TABLE IF NOT EXISTS user_exercises (
    userid INT NOT NULL,
    exerciseid INT NOT NULL,
    sets INT NOT NULL DEFAULT 0, -- Serie
    reps INT NOT NULL DEFAULT 0, -- Ripetizioni
    PRIMARY KEY (userid, exerciseid),
    FOREIGN KEY (userid) REFERENCES users(id),
    FOREIGN KEY (exerciseid) REFERENCES exercises(id)
);

CREATE TABLE IF NOT EXISTS muscles (
    id INT PRIMARY KEY,
    name VARCHAR(255) UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS exercise_muscles (
    exerciseid INT NOT NULL,
    muscleid INT NOT NULL,
    PRIMARY KEY (exerciseid, muscleid),
    FOREIGN KEY (exerciseid) REFERENCES exercises(id),
    FOREIGN KEY (muscleid) REFERENCES muscles(id)
);

CREATE TABLE IF NOT EXISTS preferred_muscles (
    userid INT NOT NULL,
    muscleid INT NOT NULL,
    PRIMARY KEY (userid, muscleid),
    FOREIGN KEY (userid) REFERENCES users(id),
    FOREIGN KEY (muscleid) REFERENCES muscles(id)
);
)

INSERT INTO users (name, surname, email, pass) VALUES ("Francesco", "Franceschini", "francesco.franceschini@fakemail.com", "password");
