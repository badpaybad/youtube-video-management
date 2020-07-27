-- --------------------------------------------------------
-- Host:                         ec2-13-229-229-126.ap-southeast-1.compute.amazonaws.com
-- Server version:               5.7.30-0ubuntu0.18.04.1 - (Ubuntu)
-- Server OS:                    Linux
-- HeidiSQL Version:             10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for moneynote
DROP DATABASE IF EXISTS `moneynote`;
CREATE DATABASE IF NOT EXISTS `moneynote` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_bin */;
USE `moneynote`;

-- Dumping structure for table moneynote.cmscategory
DROP TABLE IF EXISTS `cmscategory`;
CREATE TABLE IF NOT EXISTS `cmscategory` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Title` varchar(2048) COLLATE utf8mb4_bin DEFAULT NULL,
  `ItemsCount` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Dumping data for table moneynote.cmscategory: ~2 rows (approximately)
/*!40000 ALTER TABLE `cmscategory` DISABLE KEYS */;
REPLACE INTO `cmscategory` (`Id`, `ParentId`, `TenantId`, `IsDeleted`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `Title`, `ItemsCount`) VALUES
	('64b0bf60-3839-4803-83d2-96616dc3818b', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 01:55:16', '00000000-0000-0000-0000-000000000000', NULL, 'Kinh nghiệm mẹ bầu', 6),
	('ba2bba32-d886-4389-8e04-96ad13c7f538', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 14:22:57', '00000000-0000-0000-0000-000000000000', NULL, 'Demo ', 2);
/*!40000 ALTER TABLE `cmscategory` ENABLE KEYS */;

-- Dumping structure for table moneynote.cmscontent
DROP TABLE IF EXISTS `cmscontent`;
CREATE TABLE IF NOT EXISTS `cmscontent` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Title` longtext COLLATE utf8mb4_bin,
  `Thumbnail` longtext COLLATE utf8mb4_bin,
  `Description` longtext COLLATE utf8mb4_bin,
  `UrlRef` longtext COLLATE utf8mb4_bin,
  `CountView` bigint(20) DEFAULT NULL,
  `ThumbnailWidth` int(11) DEFAULT '0',
  `ThumbnailHeight` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Dumping data for table moneynote.cmscontent: ~11 rows (approximately)
/*!40000 ALTER TABLE `cmscontent` DISABLE KEYS */;
REPLACE INTO `cmscontent` (`Id`, `ParentId`, `TenantId`, `IsDeleted`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `Title`, `Thumbnail`, `Description`, `UrlRef`, `CountView`, `ThumbnailWidth`, `ThumbnailHeight`) VALUES
	('1b5e1a55-57db-4966-b625-75cdb34bb472', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 13:53:04', '00000000-0000-0000-0000-000000000000', NULL, '15 Thói Quen Nhỏ của Người Giàu!', 'https://i.ytimg.com/vi/_Wr_z-mc8sA/hqdefault.jpg', '15 Thói Quen Nhỏ của Người Giàu! ► 76 AUDIO + EBOOKS BÀI HỌC THÀNH CÔNG: https: / /www.baihocthanhcong.org /  ► Đăng Ký Mua  Sổ Tay GOTINO: https: / /sogotino.com / ủng hộ ADMIN Nhập Mã Giảm Giá: VIP10 - Để được giảm thêm 10%  ► Học Làm Video Giống Như ADMIN: http: / /bit.ly /lamvideoanimation  ► Fanpage: https: / /www.facebook.com /baihocthanhcongchannel /  ► Nhóm BÀI HỌC THÀNH CÔNG: https: / /www.facebook.com /groups /baihocthanhcongofficial  ► Founder NGUYỄN ĐÌNH CÔNG: https: / /www.facebook.com /congcreator  ► ADMIN KIM NGÂN: https: / /www.facebook.com /nganaudio  ► Bài Học Thành Công - Đăng Ký: https: / /goo.gl /hjhbRB Thêm Bài Học - Đổi Cuộc Đời!  ► Liên hệ tài trợ, quảng cáo: anhduy.htd@gmail.com ► SĐT: +84965524965  #baihocthanhcong , #thaydoicuocdoi  Kênh Học Tập dành cho những con người khát khao đạt được Giàu Có - Thành Công - Tự Do - Hạnh Phúc Trọn Vẹn trong cuộc sống.  Đúc kết những triết lý thành công, bài học làm giàu, cách quản lý tài chính, bí quyết sống hạnh phúc  u0026 khỏe mạnh... giúp bạn thay đổi cuộc đời. Hãy comment chủ đề, bài học, kỹ năng bạn muốn học nhé!  Music Licensed Under CC: Life of Riley by Kevin MacLeod is licensed under a Creative Commons Attribution license (https: / /creativecommons.org /licenses /by /4.0 /) Source: http: / /incompetech.com /music /royalty-free /index.html?isrc=USUAN1400054 Artist: http: / /incompetech.com /  Photos Licensed Under CC www.pexels.com www.pixabay.com  Copyright © 2018 - Bài Học Thành Công Do not Reup! ------------------------------------- Bản thân bạn đã là 1 ngọn nến đang tỏa sáng vô cùng quý giá, hãy mang ánh sáng đó thắp sáng cho nhiều người khác. 1 ngọn nến không mất gì khi thắp sáng 1 ngọn nến khác. Càng thắp sáng cho nhiều người, bạn càng thành công! ❤ Gửi đến Bạn bằng cả Trái Tim: ADMIN', 'https://www.youtube.com/watch?v=_Wr_z-mc8sA', 0, 0, 0),
	('299d25e7-de3f-4e87-9dc5-034ca86c1256', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 13:51:51', '00000000-0000-0000-0000-000000000000', NULL, '14 Mẹo Ứng Xử Đi Đâu Cũng Được Coi Trọng! ^^', 'https://i.ytimg.com/vi/QXAAF3Fj_QM/hqdefault.jpg', '14 Mẹo ứng xử giúp bạn đi đâu cũng được coi trọng! ► Kỹ Năng Giao Tiếp Thông Minh: http: / /bit.ly /giaotiep-thongminh  ► 76 AUDIO + EBOOKS BÀI HỌC THÀNH CÔNG: https: / /www.baihocthanhcong.org /  ► Đăng Ký Mua  Sổ Tay GOTINO: https: / /www.sogotino.com / ủng hộ ADMIN Nhập Mã Giảm Giá: VIP10 - Để được giảm thêm 10%  ► Học Làm Video Giống Như ADMIN: http: / /bit.ly /lamvideoanimation  ► Fanpage: https: / /www.facebook.com /baihocthanhcongchannel /  ► Nhóm BÀI HỌC THÀNH CÔNG: https: / /www.facebook.com /groups /baihocthanhcongofficial  ► Founder NGUYỄN ĐÌNH CÔNG: https: / /www.facebook.com /congcreator  ► ADMIN KIM NGÂN: https: / /www.facebook.com /nganaudio  ► Bài Học Thành Công - Đăng Ký: https: / /goo.gl /hjhbRB Thêm Bài Học - Đổi Cuộc Đời!  ► Liên hệ tài trợ, quảng cáo: anhduy.htd@gmail.com ► SĐT: +84965524965  #baihocthanhcong , #thaydoicuocdoi  Kênh Học Tập dành cho những con người khát khao đạt được Giàu Có - Thành Công - Tự Do - Hạnh Phúc Trọn Vẹn trong cuộc sống.  Đúc kết những triết lý thành công, bài học làm giàu, cách quản lý tài chính, bí quyết sống hạnh phúc  u0026 khỏe mạnh... giúp bạn thay đổi cuộc đời. Hãy comment chủ đề, bài học, kỹ năng bạn muốn học nhé!  Music Licensed Under CC: Life of Riley by Kevin MacLeod is licensed under a Creative Commons Attribution license (https: / /creativecommons.org /licenses /by /4.0 /) Source: http: / /incompetech.com /music /royalty-free /index.html?isrc=USUAN1400054 Artist: http: / /incompetech.com /  Photos Licensed Under CC www.pexels.com www.pixabay.com  Copyright © 2018 - Bài Học Thành Công Do not Reup! ------------------------------------- Bản thân bạn đã là 1 ngọn nến đang tỏa sáng vô cùng quý giá, hãy mang ánh sáng đó thắp sáng cho nhiều người khác. 1 ngọn nến không mất gì khi thắp sáng 1 ngọn nến khác. Càng thắp sáng cho nhiều người, bạn càng thành công! ❤ Gửi đến Bạn bằng cả Trái Tim: ADMIN', 'https://www.youtube.com/watch?v=QXAAF3Fj_QM', 0, 0, 0),
	('3c8c8ea8-428b-4766-a1c0-c6593f7db6b2', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 06:07:10', '00000000-0000-0000-0000-000000000000', NULL, 'Fresh Peach Milk Recipe For Hot Summer | Today I Cook', 'https://i.ytimg.com/vi/WLtWvLkp9G0/maxresdefault.jpg', '#Todayicook #Hômnaytớnấu', 'https://www.youtube.com/watch?v=WLtWvLkp9G0', 0, 0, 0),
	('4425194e-16c7-43a4-8280-6fae2ccd8c6a', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 06:07:26', '00000000-0000-0000-0000-000000000000', NULL, 'Yellow Watermelon Jelly Makes You Funny, Isn&#39;t It? | Today I Cook', 'https://i.ytimg.com/vi/QmONMf3hV8U/maxresdefault.jpg', '#Todayicook', 'https://www.youtube.com/watch?v=QmONMf3hV8U', 0, 0, 0),
	('4b2607a7-988c-4c50-aab1-be95ea3f8953', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 13:41:44', '00000000-0000-0000-0000-000000000000', NULL, 'TRỨNG CHIÊN NƯỚC MẮM làm hao nồi cơm - Món Ăn Ngon', 'https://i.ytimg.com/vi/fl6TjHB9kmI/hqdefault.jpg', 'TRỨNG CHIÊN NƯỚC MẮM làm hao nồi cơm, món trứng chiên được biến tấu đơn giản, đẹp mắt và đặc biệt rất dễ làm luôn, hương vị thì thơm ngon đậm đà, vô cùng hấp dẫn, ăn rất là bắt cơm đó nha các bạn ơi.  - NGUYÊN LIỆU: 5 TRỨNG GÀ HÀNH LÁ, ỚT SỪNG TRÂU, CỦ TỎI 4 MUỖNG NƯỚC MẮM (4 tbsp) 4 MUỖNG ĐƯỜNG (4 tbsp) 1 /2 MUỖNG NHỎ MUỐI (1 /2 tsp) 1 /2 MUỖNG NHỎ NGŨ VỊ HƯƠNG (1 /2 tsp) 1 /2 MUỖNG NHỎ TIÊU XAY (1 /2 tsp)  Danh mục món ăn ngon: http: / /bit.ly /2YDAtiT  Cách làm bánh ngon: http: / /bit.ly /2YXUf99  Thích ăn vặt: http: / /bit.ly /2WufEcd  Thích ăn lẩu: http: / /bit.ly /2JG4Cuv  Làm bánh tráng trộn: http: / /bit.ly /2YMB3uA  Các loại kẹo mứt: http: / /bit.ly /2YyD65A  Các món ngon từ trứng: http: / /bit.ly /2K6CArj  Các món ngâm chua: http: / /bit.ly /2QsXhyQ  Video nấu ăn mới nhất: http: / /bit.ly /2WmuBx7  Tham gia hội và chia sẽ món ăn của các bạn trên facebook các bạn nhé ! Fanpage: https: / /fb.com /LikeMonAnNgon Hội Món Ăn Ngon: https: / /fb.com /groups /MonAnNgonGroup', 'https://www.youtube.com/watch?v=fl6TjHB9kmI', 0, 0, 0),
	('8603f020-0275-4eeb-a766-5ea93a442995', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 14:23:58', '00000000-0000-0000-0000-000000000000', NULL, 'FAPtv Cơm Nguội: Tập 223 - Tam Quốc Diễn Nghĩa Parody', 'https://i.ytimg.com/vi/jlfcBSulKSc/maxresdefault.jpg', 'FAPtv Cơm Nguội: Tập 223 - Tam Quốc Diễn Nghĩa Parody Thiếu Niên Danh Tướng 3Q VNG – Làm chủ chiến trường, đánh không chờ lượt. FAPTV đang đợi bạn, tải game ngay: https: / /go.onelink.me /zkkg /FAPTV #FAPtv #Cơmnguội #Thieuniendanhtuong ** / / / Nếu trong khi xem phim có xuất hiện quảng cáo, hãy xem hoặc click quảng cáo để ủng hộ chúng tôi  / / /*** ►Directed by: Trần Đức Viễn ►D.O.P: Hào Wong ►1st AD: Võ Minh Lộc ►2nd AD: Thanh Sang ►Producer: Trần Long ►Script: Thái Vũ, Vinh Râu, Đức Viễn, Huỳnh Phương ►Cast: Thái Vũ, Huỳnh Phương, Vinh Râu, Uy Trần, Mai Xuân Thứ, Ribi Sachi, Quốc Trường cùng với một số diễn viên khác.... ►Cam: Duy Phong -Hoà Vũ - Thành Võ ►Lights: Thuần Phạm -Phú Quang ,Hoàng Trí ►Location Manager : Tâm Nguyễn ►Props : Hải Lê - Hoàng Trọng, Minh Tuấn ►Equipment: Đức Duy, Khánh Huy, Trí ►Caterer: Jupi Mol ►Chủ nhiệm: Tony Quốc Anh ►Sound : Trần Linh ►Editor: Lê Duy, Chấn Hưng, Lê Thanh, Diệp Khoa ►Master: Đức Viễn ►Colorist: Lê Thanh ►Marketing : Tài Nguyễn, Thuỷ Tiên, Trúc, Aki ►Assistant Director: Nguyễn Băng ►Graphics design: Phương Giao ►VFX: Diệp Khoa ►Driver: Thái ►Behind The Secenes: Đỗ Đình Huy ► LH quảng cáo - Phòng khách hàng: 0905942902 (Mr Tài) – 0968759739 (Ms Thư) ► mail: faptvgroup@gmail.com FB: đạo diễn: ► Trần Đức Viễn: https: / /facebook.com /viengalac diễn viên: ► Thái Vũ https: / /www.facebook.com /FAPtv.ThaiVu ► Ribi Sachi https: / /www.facebook.com /ribisachi ► Vinh Râu https: / /www.facebook.com /VinhRau.FAP ► Huỳnh Phương https: / /facebook.com /HuynhPhuongFAPtv ► Mai Xuân Thứ https: / /www.facebook.com /MaiXuanThu.FAPtv  fap, fap tv, faptv com nguoi, phim hai, phim hài, phim hài mới, phim hai 2020, hai moi nhat, hai huoc, com nguoi, comnguoi, faptv cơm nguội mới nhất, com nguoi,', 'https://www.youtube.com/watch?v=jlfcBSulKSc', 0, 0, 0),
	('a1c8069c-69ff-4f66-a04a-9ee849ae1e68', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 06:08:35', '00000000-0000-0000-0000-000000000000', NULL, 'Vietnamese fruits with yogurt - awesome dessert for hot summer | Today I Cook', 'https://i.ytimg.com/vi/2YvCIFSHjSQ/maxresdefault.jpg', '', 'https://www.youtube.com/watch?v=2YvCIFSHjSQ', 0, 0, 0),
	('ad9e9d3e-d8d5-40a9-bc7f-b8b92a5f24e7', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 06:07:41', '00000000-0000-0000-0000-000000000000', NULL, 'What to do with bamboo shoots? | Let make soup of fish with pickled bamboo shoots', 'https://i.ytimg.com/vi/PGExFYtjFpE/maxresdefault.jpg', '', 'https://www.youtube.com/watch?v=PGExFYtjFpE', 0, 0, 0),
	('affed93f-2d79-4075-beb5-4e880b1081e1', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 01:59:25', '00000000-0000-0000-0000-000000000000', NULL, 'transformer | cartoon trains for children | educational video | trains for kids | kids vehicles', 'https://i.ytimg.com/vi/DoeHYUy1q14/maxresdefault.jpg', 'Kidsfirst is an edutainment app brought to you by the makers of Ralph and Rocky, Transformer vehicles, Mad Beans, Road Rangers, Littler Red Car, Bob the Train, Farmees, Kids Channel, Dan the Monster truck, Haunted House Monster Truck, Schoolies and many more of the preschool content you kids enjoy. So if you can’t get enough of our videos, download the app now for unlimited fun and playschool learning! We have a nursery rhyme, song, video or game for every occasion here at Kids TV Channel. With home to many cartoon 2D and 3D characters, we are a preschooler\'s best friend. A school away from school we make our videos not just to please the toddler but also to educate him /her with new concepts, skills, and ideas. We take kindergarten a step further with an in-depth understanding of a preschoolers comprehension, cognitive development, motor skills, language acquisition,  executive functions, self-concept, identity development and moral values.  ============================================ Music and Lyrics: Copyright USP Studios™ Video: Copyright USP Studios™ ============================================', 'https://www.youtube.com/watch?v=DoeHYUy1q14', 0, 0, 0),
	('de379d79-cf69-4e22-b6bb-e2fff65f5a41', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 06:08:14', '00000000-0000-0000-0000-000000000000', NULL, 'What to do with fresh bamboo shoots? | Make pickled bamboo shoots | Stir fry bamboo shoots with meat', 'https://i.ytimg.com/vi/HDU3ZA-uFJQ/maxresdefault.jpg', '', 'https://www.youtube.com/watch?v=HDU3ZA-uFJQ', 0, 0, 0),
	('e3f2075d-4e04-4922-85cc-a000c036f312', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 05:33:13', '00000000-0000-0000-0000-000000000000', NULL, 'Cánh hoa tổn thương - Hoàng Yến Chibi | Official Music Video', 'https://i.ytimg.com/vi/Fvn141xXDwc/maxresdefault.jpg', 'CÁNH HOA TỔN THƯƠNG - HOÀNG YẾN CHIBI | OFFICIAL MUSIC VIDEO #hoangyenchibi #canhhoatonthuong #musicvideo ————————— Nghe audio “Cánh hoa tổn thương” tại • Spotify: https: / /spoti.fi /2sW6fNI • Apple Music: https: / /apple.co /2s0MDrv • Nhạc của tui: https: / /bit.ly /36waiiu ————————— 👉Lyrics Khi ánh mặt trời lui dần sau tán cây  Cảm giác cứ mỗi buổi chiều khi hoàng hôn xuống đây Từng cánh hoa mềm ướt vì rơi xuống ghé ngang đôi mi  Năm tháng cứ trôi dần em cạn khô nước mắt  Em sống trong ngàn nỗi đau  Em sẽ không thể tha thứ  Tất cả lời nói của anh là nói dối Và em chẳng biết lời hứa  Anh anh đã lừa dối đủ chưa hay vẫn còn nữa Người lạnh lùng để gió cuốn trôi tình em nhẹ như cánh hoa  Người chẳng thiết tha đánh rơi tình ta Tình em như con sóng ngoài khơi  Xa thật xa tận nơi cuối trời không ai màng tới  Cùng đắng cay trên cuộc đời 1 nơi cuối đường  Nhận ra vết thương cánh hoa tổn thương 👉Credit Minh Yến Entertainment Executive Producer : Minh Đạt Nguyễn Director: Nguyễn Khắc Huy D.O.P : Nguyễn Vinh Phúc Starring: Hoàng Yến Chibi  Pom  Soho Hoàng Sơn Trịnh Thảo  Amee Key  Nicky  Duy Khánh- Misthy Music Production Composer: Nguyễn Đình Vũ Arranger : Đoàn Minh Vũ Singer: Hoàng Yến Chibi Mix  u0026 Master: Bố Thỏ Heo MV Production Creative Director                        :  Minh Đạt Nguyễn                                                            Huy Cóc                                                          Hoàng Yến Chibi  Written by                                    : Huy Cóc                                                            Hoàng Yến Chibi 1st AD                                             : Vũ Đức Thuận  Camera Operator                        : Nghĩa Trần  Camera Assistant                        : Đinh Quốc An                                                            Lê Phương Bình                                                            Phạm Bá Duy Tường  Focus Puller                                  : Đinh Cao Trí Location Manager                       : HIệp Nguyễn Assistant Location Manager     : Thông Võ Producer                                       : Mrs Bắp  Assistant Production                  : Mine Nguyễn  Art Director                                  : Lê Đình Hoà Gaffer                                            : Minh Nhí Post Production                          : Đông Nam Movie VFX                                                :  Unn Thành                                                           Hoàng Thuận Equipment                                    : HK FILM Communication Manager         : Tân Cao Social Media Manager               : Nhật Duy Choreographer                            : Quang Đăng  Dance Trainer                              : Trần Hải Yến  Designer                                        : Nguyễn Đạt  Design Assistant                          : Nguyễn Vinh Phú                                                           Lê Nguyễn Thanh Phong Stylist                                             : Nguyễn Chí Cường Costume                                        : Khôi Minh Bảo  Costume Assistant                      : Hữu Khang                                                           Lê Nga Sword Props                                 : Vạn Kiếm Sơn Trang Makeup                                         : Team Makeup Tuấn Anh  Photo Grapher BTS                     : Tú Hoàng – Nghĩa – Dương Văn  Edit BTS                                         : Huy Lê  Graphic Designer                         : Shj Bui  Assistant                                       :  NK Hoàng – TB Vy Catering                                        : Le Le Team  Cascadeur                                     : Team X :  Mass Actor Manager                  : Vương Thiên Ý  Driver                                             : Mr.Tài Crane Truck                                  : An Khang Company  Special thanks to                           Khu Bảo Tồn Thiên Nhiên BÌnh Châu – Phước Bửu , Xuyên Mộc , Bà Rịa Vũng Tàu   Our Sponsor: Vietjetair.com  -------------------- Đăng ký kênh để xem nhiều video mới tại :  http: / /metub.net /hoangyenchibi Theo dõi Hoàng Yến Chi Bi trên ►FANPAGE : https: / /www.facebook.com /hoangyenfan / --------------------- © Bản quyền thuộc về Hoàng Yến Chi Bi © Copyright by Hoang Yen Chi Bi ☞ Do not Reup', 'https://www.youtube.com/watch?v=Fvn141xXDwc', 0, 0, 0);
/*!40000 ALTER TABLE `cmscontent` ENABLE KEYS */;

-- Dumping structure for table moneynote.cmsrelation
DROP TABLE IF EXISTS `cmsrelation`;
CREATE TABLE IF NOT EXISTS `cmsrelation` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `CategoryId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `ContentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Dumping data for table moneynote.cmsrelation: ~8 rows (approximately)
/*!40000 ALTER TABLE `cmsrelation` DISABLE KEYS */;
REPLACE INTO `cmsrelation` (`Id`, `ParentId`, `TenantId`, `IsDeleted`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `CategoryId`, `ContentId`) VALUES
	('4faab94e-3059-49d7-9ba8-15e79e75a0f4', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-07-03 09:15:08', '00000000-0000-0000-0000-000000000000', NULL, '64b0bf60-3839-4803-83d2-96616dc3818b', 'de379d79-cf69-4e22-b6bb-e2fff65f5a41'),
	('5e6b9ec5-857b-4d30-a14e-310847121243', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 01:55:52', '00000000-0000-0000-0000-000000000000', NULL, '64b0bf60-3839-4803-83d2-96616dc3818b', '1b5e1a55-57db-4966-b625-75cdb34bb472'),
	('c0bc6a7b-8028-4c45-8d5a-004b1340a22d', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 01:55:46', '00000000-0000-0000-0000-000000000000', NULL, '64b0bf60-3839-4803-83d2-96616dc3818b', '4b2607a7-988c-4c50-aab1-be95ea3f8953'),
	('c0d8190c-1c35-4e54-85f7-16128fcf0c60', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 01:59:34', '00000000-0000-0000-0000-000000000000', NULL, '64b0bf60-3839-4803-83d2-96616dc3818b', 'affed93f-2d79-4075-beb5-4e880b1081e1'),
	('d6f16cc8-41d7-44eb-a8cc-ef649e57fbe2', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 01:55:55', '00000000-0000-0000-0000-000000000000', NULL, '64b0bf60-3839-4803-83d2-96616dc3818b', '8603f020-0275-4eeb-a766-5ea93a442995'),
	('e84a5ba4-7420-4580-9a13-378d38f66a5f', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-24 16:13:51', '00000000-0000-0000-0000-000000000000', NULL, 'ba2bba32-d886-4389-8e04-96ad13c7f538', 'a1c8069c-69ff-4f66-a04a-9ee849ae1e68'),
	('f9534647-e768-4e24-8080-fa6f56439345', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-07-03 09:15:08', '00000000-0000-0000-0000-000000000000', NULL, 'ba2bba32-d886-4389-8e04-96ad13c7f538', 'de379d79-cf69-4e22-b6bb-e2fff65f5a41'),
	('fd2eb65c-1e44-4a75-a3c0-d2d63d310aec', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 01:58:57', '00000000-0000-0000-0000-000000000000', NULL, '64b0bf60-3839-4803-83d2-96616dc3818b', '299d25e7-de3f-4e87-9dc5-034ca86c1256');
/*!40000 ALTER TABLE `cmsrelation` ENABLE KEYS */;

-- Dumping structure for table moneynote.sysmodule
DROP TABLE IF EXISTS `sysmodule`;
CREATE TABLE IF NOT EXISTS `sysmodule` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Code` varchar(128) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Dumping data for table moneynote.sysmodule: ~5 rows (approximately)
/*!40000 ALTER TABLE `sysmodule` DISABLE KEYS */;
REPLACE INTO `sysmodule` (`Id`, `ParentId`, `TenantId`, `IsDeleted`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `Code`) VALUES
	('0ececfaa-1c3b-4d72-9904-71cf08cbcfc3', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 00:04:12', '00000000-0000-0000-0000-000000000000', NULL, 'CmsCategory'),
	('29cd43d3-5413-4272-8153-ed4d5bc05881', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 22:35:29', '00000000-0000-0000-0000-000000000000', NULL, 'ApiYoutubeContent'),
	('5013bf3c-e11f-4794-bb46-9c9d8cf2e5ea', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 00:04:17', '00000000-0000-0000-0000-000000000000', NULL, 'AdminDashboard'),
	('5d79cb45-a5b5-4ddc-9d61-c625db34d45c', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 00:04:13', '00000000-0000-0000-0000-000000000000', NULL, 'CmsContent'),
	('b374e6a0-d33d-423f-8fe6-5c185fffa0f2', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 00:04:17', '00000000-0000-0000-0000-000000000000', NULL, 'AdminDashboard');
/*!40000 ALTER TABLE `sysmodule` ENABLE KEYS */;

-- Dumping structure for table moneynote.syspermission
DROP TABLE IF EXISTS `syspermission`;
CREATE TABLE IF NOT EXISTS `syspermission` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Code` varchar(128) COLLATE utf8mb4_bin DEFAULT NULL,
  `ModuleCode` varchar(128) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Dumping data for table moneynote.syspermission: ~4 rows (approximately)
/*!40000 ALTER TABLE `syspermission` DISABLE KEYS */;
REPLACE INTO `syspermission` (`Id`, `ParentId`, `TenantId`, `IsDeleted`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `Code`, `ModuleCode`) VALUES
	('21d81047-aaa9-4c3a-ac83-f90dc7bfaec2', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 00:04:10', '00000000-0000-0000-0000-000000000000', NULL, '*', 'AdminDashboard'),
	('24060d84-e695-40a0-9cca-1eeeffbb71f8', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-18 22:35:29', '00000000-0000-0000-0000-000000000000', NULL, '*', 'ApiYoutubeContent'),
	('72b5af78-bd16-444c-894f-41a5110990ab', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 00:04:13', '00000000-0000-0000-0000-000000000000', NULL, '*', 'CmsContent'),
	('ec076e0e-bfb1-47db-bf45-5c2b6a2b4f40', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 00:04:11', '00000000-0000-0000-0000-000000000000', NULL, '*', 'CmsCategory');
/*!40000 ALTER TABLE `syspermission` ENABLE KEYS */;

-- Dumping structure for table moneynote.user
DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Username` varchar(128) COLLATE utf8mb4_bin DEFAULT NULL,
  `Password` varchar(512) COLLATE utf8mb4_bin DEFAULT NULL,
  `LastLogedin` datetime DEFAULT NULL,
  `LastToken` varchar(512) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Dumping data for table moneynote.user: ~1 rows (approximately)
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
REPLACE INTO `user` (`Id`, `ParentId`, `TenantId`, `IsDeleted`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `Username`, `Password`, `LastLogedin`, `LastToken`) VALUES
	('2a2d6c61-ee99-48f5-9e9f-d2dcbf4ebe48', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0, '00000000-0000-0000-0000-000000000000', '2020-06-17 00:04:09', '00000000-0000-0000-0000-000000000000', NULL, 'supperadmin', 'XzdXaEpVywr1Yz0ynxI/ZMqRxYEurYDMv9CfeugPvwPjVRmHRGiK0f8oDOqo3CJJ/TGM4lV4hQ9lFN+wTxzBqA==', '2020-07-10 13:40:51', 'J89usgaRFcRirvy+ksHzvhMWbyDdaVUhqg3N6SWPvPIdn/EhI4kzKJZ98HWVStOJ1lAhfMPkr7F+aXwzh5M18w==');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;

-- Dumping structure for table moneynote.useracl
DROP TABLE IF EXISTS `useracl`;
CREATE TABLE IF NOT EXISTS `useracl` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `UserId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `ModuleCode` varchar(45) COLLATE utf8mb4_bin DEFAULT NULL,
  `PermissionCode` varchar(45) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Dumping data for table moneynote.useracl: ~0 rows (approximately)
/*!40000 ALTER TABLE `useracl` DISABLE KEYS */;
/*!40000 ALTER TABLE `useracl` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
