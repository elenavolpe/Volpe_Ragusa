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
    age INT UNSIGNED NOT NULL DEFAULT 0 CHECK (age >= 0),
    admin BOOLEAN DEFAULT FALSE,
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
    PRIMARY KEY (userid, exerciseid),
    FOREIGN KEY (userid) REFERENCES users(id),
    FOREIGN KEY (exerciseid) REFERENCES exercises(id)
);

CREATE TABLE IF NOT EXISTS muscles (
    id INT AUTO_INCREMENT PRIMARY KEY ,
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
INSERT INTO users (name, surname, email, pass, admin) VALUES ("Admin", "Admin", "Admin@mail.it", "admin", TRUE);

INSERT INTO muscles(name) VALUES ("Quadricipiti");
INSERT INTO muscles(name) VALUES ("Glutei");
INSERT INTO muscles(name) VALUES ("Addominali");
INSERT INTO muscles(name) VALUES ("Pettorali");
INSERT INTO muscles(name) VALUES ("Deltoidi");
INSERT INTO muscles(name) VALUES ("Tricipiti");
INSERT INTO muscles(name) VALUES ("Schiena");
INSERT INTO muscles(name) VALUES ("Bicipiti");
INSERT INTO muscles(name) VALUES ("Trapezi");
INSERT INTO muscles(name) VALUES ("Muscoli cardiovascolari");
INSERT INTO muscles(name) VALUES ("Polpacci");

INSERT INTO exercises(name, description) VALUES ("Squat", "Posiziona una barra sulla parte superiore della schiena, spingi i fianchi all'indietro e piega le ginocchia per abbassarti. Mantieni il peso sui talloni e ritorna in posizione eretta.");
SET @id = (SELECT id FROM exercises WHERE name="Squat");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,1);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,2);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,3);

INSERT INTO exercises(name, description) VALUES ("Panca piana", "Sdraiato su una panca, solleva e abbassa un bilanciere sopra il petto. Mantieni i piedi a terra e i gomiti piegati a 90 gradi.");
SET @id = (SELECT id FROM exercises WHERE name="Panca piana");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,4);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,5);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,6);

INSERT INTO exercises(name, description) VALUES ("Stacchi da terra (Deadlift)", "    Piegati in avanti alle anche, afferra un bilanciere e solleva il peso portando il torso in posizione eretta. Contrai i glutei alla fine del movimento.");
SET @id = (SELECT id FROM exercises WHERE name="Stacchi da terra (Deadlift)");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,7);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,2);

INSERT INTO exercises(name, description) VALUES ("Flessioni", "Posiziona le mani a terra leggermente più larghe delle spalle, abbassa e solleva il corpo piegando i gomiti. Mantieni il corpo in linea retta.");
SET @id = (SELECT id FROM exercises WHERE name="Flessioni");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,4);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,5);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,6);

INSERT INTO exercises(name, description) VALUES ("Crunch", "Sdraiato sulla schiena, piega le ginocchia e solleva la parte superiore del corpo verso le ginocchia, contrai gli addominali.");
SET @id = (SELECT id FROM exercises WHERE name="Crunch");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,3);

INSERT INTO exercises(name, description) VALUES ("Trazioni alla sbarra", "Aggrappati a una sbarra e solleva il corpo fino a che il mento supera la sbarra. Mantieni i gomiti piegati.");
SET @id = (SELECT id FROM exercises WHERE name="Trazioni alla sbarra");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,7);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,8);

INSERT INTO exercises(name, description) VALUES ("Affondi", "Con un piede avanti e uno indietro, abbassati flettendo entrambe le ginocchia. Alterna le gambe.");
SET @id = (SELECT id FROM exercises WHERE name="Affondi");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,1);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,2);

INSERT INTO exercises(name, description) VALUES ("Shoulder Press", "Solleva pesi sopra la testa tenendo i gomiti leggermente piegati. Torna alla posizione di partenza.");
SET @id = (SELECT id FROM exercises WHERE name="Shoulder Press");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,5);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,6);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,9);

INSERT INTO exercises(name, description) VALUES ("Curl con manubri", "Con i gomiti aderenti al corpo, solleva i manubri contraggendo i bicipiti. Torna alla posizione iniziale.");
SET @id = (SELECT id FROM exercises WHERE name="Curl con manubri");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,8);

INSERT INTO exercises(name, description) VALUES ("Addominali laterali", "Sdraiato su un fianco, solleva il busto lateralmente contraggendo gli addominali.");
SET @id = (SELECT id FROM exercises WHERE name="Addominali laterali");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,3);

INSERT INTO exercises(name, description) VALUES ("Leg Press", "Posizionati su una macchina per la leg press, spingi un carico verso l'alto con i piedi, estendendo le ginocchia.");
SET @id = (SELECT id FROM exercises WHERE name="Leg Press");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,1);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,2);

INSERT INTO exercises(name, description) VALUES ("Pull-up inversa (Reverse Grip Pull-up)", "Aggrappati a una sbarra con le mani in presa inversa (palme rivolte verso di te) e solleva il corpo fino a che il mento supera la sbarra.");
SET @id = (SELECT id FROM exercises WHERE name="Pull-up inversa (Reverse Grip Pull-up)");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,7);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,8);

INSERT INTO exercises(name, description) VALUES ("Plank", "Posizionati a terra in posizione di push-up e mantieni il corpo in una linea retta, sostenendoti sugli avambracci e sulla punta dei piedi.");
SET @id = (SELECT id FROM exercises WHERE name="Plank");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,3);

INSERT INTO exercises(name, description) VALUES ("Pulldown al cavo (Lat Pulldown)", "Seduto alla macchina del lat pulldown, afferra la barra e tira verso il petto, mantenendo la schiena dritta.");
SET @id = (SELECT id FROM exercises WHERE name="Pulldown al cavo (Lat Pulldown)");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,7);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,8);

INSERT INTO exercises(name, description) VALUES ("Step-ups", "Utilizzando un banco o una piattaforma, alterna l'innalzamento di una gamba alla volta, portandola sopra la superficie.");
SET @id = (SELECT id FROM exercises WHERE name="Step-ups");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,1);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,2);

INSERT INTO exercises(name, description) VALUES ("Burpees", "Da una posizione eretta, abbassati in una posizione di plank, fai una flessione, riporta le gambe sotto di te e salta in alto.");
SET @id = (SELECT id FROM exercises WHERE name="Burpees");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,4);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,5);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,10);

INSERT INTO exercises(name, description) VALUES ("Calf Raises", "In piedi con i piedi allineati, solleva i talloni verso l'alto contraggendo i muscoli del polpaccio.");
SET @id = (SELECT id FROM exercises WHERE name="Calf Raises");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,11);

INSERT INTO exercises(name, description) VALUES ("Russian Twist", "Sdraiato sulla schiena, solleva le gambe e ruota il busto da un lato all'altro, toccando il pavimento con le mani.");
SET @id = (SELECT id FROM exercises WHERE name="Russian Twist");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,3);

INSERT INTO exercises(name, description) VALUES ("Dips", "Utilizzando parallele o una sedia, abbassati e sollevati utilizzando i tricipiti per spingere il corpo verso l'alto.");
SET @id = (SELECT id FROM exercises WHERE name="Dips");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,4);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,6);

INSERT INTO exercises(name, description) VALUES ("Mountain Climbers", "In posizione di plank, porta alternativamente le ginocchia al petto in un movimento rapido.");
SET @id = (SELECT id FROM exercises WHERE name="Mountain Climbers");
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,3);
INSERT INTO exercise_muscles(userid,muscleid) VALUES (@id,10);