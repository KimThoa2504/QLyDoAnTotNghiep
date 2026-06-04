CREATE DATABASE IF NOT EXISTS UniversityResearchManagement;

USE UniversityResearchManagement;

-- Bảng Quản lý Khoa
CREATE TABLE Faculties (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL UNIQUE,
    description TEXT
);

-- Bảng Quản lý Người dùng
CREATE TABLE Users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    full_name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    role ENUM('Admin', 'Lecturer') NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Bảng Quản lý Đề tài
CREATE TABLE Projects (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    start_date DATE,
    end_date DATE,
    status ENUM('Pending','InProgress','Completed','Cancelled') DEFAULT 'Pending',
    faculty_id INT,
    FOREIGN KEY (faculty_id) REFERENCES Faculties(id) ON DELETE SET NULL
);

-- Bảng Thành viên Đề tài
CREATE TABLE ProjectMembers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    project_id INT NOT NULL,
    masinhvien VARCHAR(50),
	hovaten VARCHAR(255),
    role ENUM('Leader', 'Member', 'Supporter') DEFAULT 'Member',
    FOREIGN KEY (project_id) REFERENCES Projects(id) ON DELETE CASCADE,
    UNIQUE(project_id, masinhvien)
);

-- Bảng Hội đồng Đánh giá
CREATE TABLE EvaluationBoards (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT
);

-- Bảng Đánh giá
CREATE TABLE Evaluations (
    id INT AUTO_INCREMENT PRIMARY KEY,
    project_id INT NOT NULL,
    board_id INT NOT NULL,
    evaluation_date DATE,
    comments TEXT,
    score DECIMAL(5,2),
    FOREIGN KEY (project_id) REFERENCES Projects(id) ON DELETE CASCADE,
    FOREIGN KEY (board_id) REFERENCES EvaluationBoards(id) ON DELETE CASCADE
);
-- Bảng thành viên hội đồng
CREATE TABLE BoardMembers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    board_id INT NOT NULL,
    user_id INT NOT NULL,
    role ENUM('Chairman', 'Secretary', 'Member') DEFAULT 'Member',
    FOREIGN KEY (board_id) REFERENCES EvaluationBoards(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE,
    UNIQUE(board_id, user_id)
);
-- Bảng Tài liệu
CREATE TABLE Documents (
    id INT AUTO_INCREMENT PRIMARY KEY,
    project_id INT NOT NULL,
    file_name VARCHAR(255) NOT NULL,
    file_path VARCHAR(500) NOT NULL,
    uploaded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (project_id) REFERENCES Projects(id) ON DELETE CASCADE
);
ALTER TABLE Documents
ADD COLUMN public_id VARCHAR(255) NULL;
ALTER TABLE Documents
ADD COLUMN file_type VARCHAR(50) NULL,
ADD COLUMN file_size BIGINT NOT NULL DEFAULT 0;


-- Bảng Báo cáo
CREATE TABLE Reports (
    id INT AUTO_INCREMENT PRIMARY KEY,
    report_type ENUM('Annual', 'Progress', 'Custom') NOT NULL,
    generated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    description TEXT
);

