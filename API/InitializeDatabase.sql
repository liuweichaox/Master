create table Customers
(
    Id                  char(36) charset ascii not null
        primary key,
    Email               longtext               not null comment '邮箱',
    Name                longtext               not null comment '姓名',
    WelcomeEmailWasSent tinyint(1)             not null comment '是否发送过欢迎邮件'
);

create table InternalCommands
(
    Id            char(36) charset ascii                           not null
        primary key,
    Type          longtext                                         not null,
    Data          longtext                                         not null,
    EnqueueDate   datetime(6) default '0001-01-01 00:00:00.000000' not null,
    ProcessedDate datetime(6)                                      null
);

create table OutboxMessages
(
    Id            char(36) charset ascii not null
        primary key,
    OccurredOn    datetime(6)            not null,
    Type          longtext               not null,
    Data          longtext               not null,
    ProcessedDate datetime(6)            null
);

