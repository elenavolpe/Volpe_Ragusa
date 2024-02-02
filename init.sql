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
    pass VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS exercises (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) UNIQUE,
    description VARCHAR(255) NOT NULL,
    popularity_score INT DEFAULT 0 NOT NULL,
    last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS workoutplans (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description VARCHAR(255) NOT NULL,
    userid INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (name, description, userid),
    FOREIGN KEY (userid) REFERENCES users(id)
);

-- Tabella di relazione uno a molti tra workoutplan ed exercise
CREATE TABLE IF NOT EXISTS workoutplan_exercises (
    workoutplanid INT NOT NULL,
    exerciseid INT NOT NULL,
    sets INT NOT NULL, -- Serie
    reps INT NOT NULL, -- Ripetizioni
    PRIMARY KEY (workoutplanid, exerciseid),
    FOREIGN KEY (workoutplanid) REFERENCES workoutplans(id),
    FOREIGN KEY (exerciseid) REFERENCES exercises(id)
);

INSERT INTO users (name, surname, email, pass) VALUES ("Francesco", "Franceschini", "francesco.franceschini@fakemail.com", "password");
