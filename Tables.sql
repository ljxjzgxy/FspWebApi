create table users(
	userid character varying(20) PRIMARY KEY,
	pwd character(44) NOT NULL,
	first_name character varying(15),
	last_name character varying(15),
	email character varying(128),
	test_mode boolean NOT NULL DEFAULT FALSE,
	created timestamp with time zone DEFAULT CURRENT_TIMESTAMP
)