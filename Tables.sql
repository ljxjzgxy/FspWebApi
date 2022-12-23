create table users(
	userid character varying(20),
	password_hash character varying(44) NOT NULL,
	first_name character varying(15),
	last_name character varying(15),
	email character varying(128),
	test_mode boolean NOT NULL DEFAULT FALSE,
	created timestamp with time zone DEFAULT CURRENT_TIMESTAMP PRIMARY KEY
);

create unique index unique_idx_lower_userid on users(lower(userid));
create unique index unique_idx_lower_email on users(lower(email));