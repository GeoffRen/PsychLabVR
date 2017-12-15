/* drop table if exists BasicAnswer;
drop table if exists DASSAnswer;
drop table if exists SPINAnswer;
drop table if exists SpokeAloudAnswer;
drop table if exists WSAPAnswer;
drop table if exists Vision;
drop table if exists Location;
drop table if exists LeftControllerLocation;
drop table if exists RightControllerLocation;
drop table if exists Session;
drop table if exists Subject;
drop table if exists Scene;
drop table if exists Project; */

create table if not exists Subject(
	SubjectId int auto_increment,
    Age varchar(255),
	Ethnicity varchar(255),
	Education varchar(255),
    Marital varchar(255),
    Employment varchar(255),
    primary key(SubjectId)
);

create table if not exists Project(
    ProjectName varchar(255),
    Description text,
    primary key(ProjectName)
);

create table if not exists Scene(
	SceneId int auto_increment,
    ProjectName varchar(255) not null,
    SceneName varchar(255) not null,
    Description text,
    primary key(SceneId),
    foreign key(ProjectName) references Project(ProjectName) on delete cascade on update cascade
);

create table if not exists Session(
	SessionId int auto_increment,
    StartTime datetime not null,
    SubjectId int not null,
    SceneId int not null,
    primary key(SessionId),
    foreign key(SubjectId) references Subject(SubjectId) on delete cascade on update cascade,
    foreign key(SceneId) references Scene(SceneId) on delete cascade on update cascade
);

create table if not exists BasicAnswer(
	SessionId int,
    Question varchar(255),
    Answer varchar(255) not null,
    AnswerType varchar(255) not null,
	AnswerTime datetime not null,
    ReactionTime double not null,
    primary key(SessionId, Question),
    foreign key(SessionId) references Session(SessionId) on delete cascade on update cascade
);

create table if not exists DASSAnswer(
	SessionId int,
    Question varchar(255),
    Answer varchar(255) not null,
	AnswerTime datetime not null,
    ReactionTime double not null,
    primary key(SessionId, Question),
    foreign key(SessionId) references Session(SessionId) on delete cascade on update cascade
);

create table if not exists SPINAnswer(
	SessionId int,
    Question varchar(255),
    
    Answer varchar(255) not null,
	AnswerTime datetime not null,
    ReactionTime double not null,
    primary key(SessionId, Question),
    foreign key(SessionId) references Session(SessionId) on delete cascade on update cascade
);

create table if not exists SpokeAloudAnswer(
	SessionId int,
    Question varchar(255),
    Answer varchar(255) not null,
	AnswerTime datetime not null,
    ReactionTime double not null,
    primary key(SessionId, Question),
    foreign key(SessionId) references Session(SessionId) on delete cascade on update cascade
);

create table if not exists WSAPAnswer(
	SessionId int,
    Question varchar(255),
    Word varchar(255) not null,
    Positive bool not null,
	Answer bool not null,
	AnswerTime datetime not null,
    ReactionTime double not null,
    primary key(SessionId, Question, Word),
    foreign key(SessionId) references Session(SessionId) on delete cascade on update cascade
);

create table if not exists Vision(
	SessionId int,
    InsertTime datetime not null,
    Object varchar(255) not null,
    xPosition double(10,6) not null,
    yPosition double(10,6) not null,
    zPosition double(10,6) not null,
    primary key(SessionId, InsertTime),
    foreign key(SessionId) references Session(SessionId) on delete cascade on update cascade
);

create table if not exists Location(
	SessionId int,
    InsertTime datetime not null,
    xPosition double(10,6) not null,
    yPosition double(10,6) not null,
    zPosition double(10,6) not null,
    primary key(SessionId, InsertTime),
    foreign key(SessionId) references Session(SessionId) on delete cascade on update cascade
);

create table if not exists LeftControllerLocation(
	SessionId int,
    InsertTime datetime not null,
    xPosition double(10,6) not null,
    yPosition double(10,6) not null,
    zPosition double(10,6) not null,
    primary key(SessionId, InsertTime),
    foreign key(SessionId) references Session(SessionId) on delete cascade on update cascade
);

create table if not exists RightControllerLocation(
	SessionId int,
    InsertTime datetime not null,
    xPosition double(10,6) not null,
    yPosition double(10,6) not null,
    zPosition double(10,6) not null,
    primary key(SessionId, InsertTime),
    foreign key(SessionId) references Session(SessionId) on delete cascade on update cascade
);


insert into Project values('Foundation', 'First project consisting of a demographic survey, basic interations, the DASS and SPIN surveys, and the WSAP');
insert into Scene values(null, 'Foundation', 'Intro', 'Intro scene acquiring consent and demographic data');
insert into Scene values(null, 'Foundation', 'Basic', 'Scene with avatar interactions');
insert into Scene values(null, 'Foundation', 'Surveys', 'Scene containing DASS and SPIN surveys');
insert into Scene values(null, 'Foundation', 'WSAP', 'The Word Sentence Assication Paradigm');