
create table if not exists play(
	id 					serial 	not null,
	name 				varchar not null,
	description 		text 	not null,
	picture_data 		bytea 	null,
	picture_extension 	varchar null,
	CONSTRAINT PLAY_PKEY PRIMARY KEY (id)
);

create table if not exists hall (
	id 					serial 	not null,
	name 				varchar not null,
	size 				int 	not null,
	CONSTRAINT HALL_PKEY PRIMARY KEY (id),
	CONSTRAINT HALL_SIZE_MORE_THEN_ZERO CHECK (size > 0)
);

create table if not exists session (
	id 			serial 		not null,
	date_from 	timestamp  	not null,
	date_to 	timestamp 	not null,
	play_id 	int 		not null,
	hall_id 	int 		not null,
	CONSTRAINT SESSION_PKEY PRIMARY KEY (id),
	CONSTRAINT SESSION_PLAY_FKEY FOREIGN KEY (play_id) REFERENCES play(id) ON DELETE CASCADE,
	CONSTRAINT SESSION_HALL_FKEY FOREIGN KEY (hall_id) REFERENCES hall(id) ON DELETE CASCADE
);

create table if not exists place (
	id 		serial 	not null,
	hall_id int 	not null,
	name 	varchar not null,
	CONSTRAINT PLACE_PKEY PRIMARY KEY (id),
	CONSTRAINT PLACE_HALL_FKEY FOREIGN KEY (hall_id) REFERENCES hall(id) ON DELETE CASCADE
);

create table if not exists users (
	id 				serial 	not null,
	login 			varchar not null,
	name 			varchar not null,
	salt 			varchar not null,
	password_hash 	varchar not null,
	is_admin 		boolean not null default false,
	CONSTRAINT USERS_PKEY PRIMARY KEY (id),
	CONSTRAINT USERS_LOGIN_UNIQUE UNIQUE (login)
);

create table if not exists ticket (
	id 			serial 	not null,
	session_id 	int 	not null,
	place_id 	int 	not null,
	is_sold 	bool 	not null default false,
	CONSTRAINT TICKET_PKEY PRIMARY KEY (id),
	CONSTRAINT TICKET_SESSION_FKEY FOREIGN KEY (session_id) REFERENCES session(id) ON DELETE CASCADE,
	CONSTRAINT TICKET_PLACE_FKEY FOREIGN KEY (place_id) REFERENCES place(id) ON DELETE CASCADE
);

