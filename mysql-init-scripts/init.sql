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
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (1,1);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (1,2);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (1,3);

INSERT INTO exercises(name, description) VALUES ("Panca piana", "Sdraiato su una panca, solleva e abbassa un bilanciere sopra il petto. Mantieni i piedi a terra e i gomiti piegati a 90 gradi.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (2,4);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (2,5);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (2,6);

INSERT INTO exercises(name, description) VALUES ("Stacchi da terra (Deadlift)", "    Piegati in avanti alle anche, afferra un bilanciere e solleva il peso portando il torso in posizione eretta. Contrai i glutei alla fine del movimento.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (3,7);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (3,2);

INSERT INTO exercises(name, description) VALUES ("Flessioni", "Posiziona le mani a terra leggermente più larghe delle spalle, abbassa e solleva il corpo piegando i gomiti. Mantieni il corpo in linea retta.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (4,4);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (4,5);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (4,6);

INSERT INTO exercises(name, description) VALUES ("Crunch", "Sdraiato sulla schiena, piega le ginocchia e solleva la parte superiore del corpo verso le ginocchia, contrai gli addominali.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (5,3);

INSERT INTO exercises(name, description) VALUES ("Trazioni alla sbarra", "Aggrappati a una sbarra e solleva il corpo fino a che il mento supera la sbarra. Mantieni i gomiti piegati.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (6,7);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (6,8);

INSERT INTO exercises(name, description) VALUES ("Affondi", "Con un piede avanti e uno indietro, abbassati flettendo entrambe le ginocchia. Alterna le gambe.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (7,1);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (7,2);

INSERT INTO exercises(name, description) VALUES ("Shoulder Press", "Solleva pesi sopra la testa tenendo i gomiti leggermente piegati. Torna alla posizione di partenza.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (8,5);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (8,6);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (8,9);

INSERT INTO exercises(name, description) VALUES ("Curl con manubri", "Con i gomiti aderenti al corpo, solleva i manubri contraggendo i bicipiti. Torna alla posizione iniziale.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (9,8);

INSERT INTO exercises(name, description) VALUES ("Addominali laterali", "Sdraiato su un fianco, solleva il busto lateralmente contraggendo gli addominali.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (10,3);

INSERT INTO exercises(name, description) VALUES ("Leg Press", "Posizionati su una macchina per la leg press, spingi un carico verso l'alto con i piedi, estendendo le ginocchia.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (11,1);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (11,2);

INSERT INTO exercises(name, description) VALUES ("Pull-up inversa (Reverse Grip Pull-up)", "Aggrappati a una sbarra con le mani in presa inversa (palme rivolte verso di te) e solleva il corpo fino a che il mento supera la sbarra.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (12,7);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (12,8);

INSERT INTO exercises(name, description) VALUES ("Plank", "Posizionati a terra in posizione di push-up e mantieni il corpo in una linea retta, sostenendoti sugli avambracci e sulla punta dei piedi.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (13,3);

INSERT INTO exercises(name, description) VALUES ("Pulldown al cavo (Lat Pulldown)", "Seduto alla macchina del lat pulldown, afferra la barra e tira verso il petto, mantenendo la schiena dritta.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (14,7);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (14,8);

INSERT INTO exercises(name, description) VALUES ("Step-ups", "Utilizzando un banco o una piattaforma, alterna l'innalzamento di una gamba alla volta, portandola sopra la superficie.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (15,1);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (15,2);

INSERT INTO exercises(name, description) VALUES ("Burpees", "Da una posizione eretta, abbassati in una posizione di plank, fai una flessione, riporta le gambe sotto di te e salta in alto.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (16,4);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (16,5);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (16,10);

INSERT INTO exercises(name, description) VALUES ("Calf Raises", "In piedi con i piedi allineati, solleva i talloni verso l'alto contraggendo i muscoli del polpaccio.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (17,11);

INSERT INTO exercises(name, description) VALUES ("Russian Twist", "Sdraiato sulla schiena, solleva le gambe e ruota il busto da un lato all'altro, toccando il pavimento con le mani.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (18,3);

INSERT INTO exercises(name, description) VALUES ("Dips", "Utilizzando parallele o una sedia, abbassati e sollevati utilizzando i tricipiti per spingere il corpo verso l'alto.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (19,4);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (19,6);

INSERT INTO exercises(name, description) VALUES ("Mountain Climbers", "In posizione di plank, porta alternativamente le ginocchia al petto in un movimento rapido.");
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (20,3);
INSERT INTO exercise_muscles(exerciseid,muscleid) VALUES (20,10);