-- Users
INSERT INTO AspNetUsers (Id,Role,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount) VALUES
(N'00000000-0000-0000-0000-000000000001',1,N'admin@podcast.local',N'ADMIN@PODCAST.LOCAL',N'admin@podcast.local',N'ADMIN@PODCAST.LOCAL',1,NULL,NEWID(),NEWID(),0,0,0,0),
(N'00000000-0000-0000-0000-000000000010',0,N'ohm',N'OHM',N'ohm@podcast.local',N'OHM@PODCAST.LOCAL',1,NULL,NEWID(),NEWID(),0,0,0,0),
(N'00000000-0000-0000-0000-000000000011',0,N'harvend',N'HARVEND',N'harvend@podcast.local',N'HARVEND@PODCAST.LOCAL',1,NULL,NEWID(),NEWID(),0,0,0,0);

-- Podcasts
INSERT INTO Podcasts (Title,Description,CreatorID,CreatedDate) VALUES
(N'6 Minute English',N'A short English-learning podcast with bite-sized episodes',N'00000000-0000-0000-0000-000000000010','2017-09-07'),
(N'8 Minute English',N'Practical English lessons in 8 minutes',N'00000000-0000-0000-0000-000000000010','2024-01-28'),
(N'To Your Inner Child',N'Guided conversations about inner child work',N'00000000-0000-0000-0000-000000000011','2024-03-20'),
(N'Podcast and Chill',N'Relaxed conversations about language learning and memory',N'00000000-0000-0000-0000-000000000011','2024-01-26'),
(N'Learning English for Work',N'English for the workplace: emails, meetings and more',N'00000000-0000-0000-0000-000000000001','2024-04-28');

-- Episodes
INSERT INTO Episodes (PodcastID,Title,ReleaseDate,Duration,PlayCount,AudioFileURL,VideoFileURL,Topic,Host) VALUES
((SELECT PodcastID FROM Podcasts WHERE Title=N'6 Minute English'), N'The benefits of doing nothing','2023-06-15',22,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/1.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/1.mp4',N'Wellbeing',N'BBC Learning English'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'6 Minute English'), N'Did a civilisation exist on Earth before humans','2025-10-23',26,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/2.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/2.mp4',N'Prehistory',N'BBC Learning English'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'8 Minute English'), N'Why Do You Always Feel Tired','2024-12-21',8,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/3.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/3.mp4',N'Health',N'Learn English Podcast'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'To Your Inner Child'), N'Everything Happens For A Reason','2024-03-20',30,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/4.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/4.mp4',N'Personal Growth',N'Learn English Podcast'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'Podcast and Chill'), N'Why We Can''t Remember New Words','2024-06-25',28,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/5.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/5.mp4',N'Memory & Language',N'Learn English Podcast'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'Learning English for Work'), N'Work emails — Office English (Episode 1)','2024-04-28',18,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/6.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/6.mp4',N'Business English',N'BBC Learning English');
