using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccessLayer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADMINISTRATORS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdmName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Cpf = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Passcode = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMINISTRATORS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CLASSES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ClassShift = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLASSES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SUBJECTS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Frequency = table.Column<int>(type: "int", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUBJECTS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TEACHERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Cpf = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Passcode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEACHERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "STUDENTS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    Cpf = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Register = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Passcode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ClassID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_STUDENTS_CLASSES_ClassID",
                        column: x => x.ClassID,
                        principalTable: "CLASSES",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LESSONS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonDate = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClassID = table.Column<int>(type: "int", nullable: false),
                    Shift = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TeacherID = table.Column<int>(type: "int", nullable: false),
                    SubjectID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LESSONS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LESSON_SUBJECT",
                        column: x => x.SubjectID,
                        principalTable: "SUBJECTS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LESSON_TEACHER",
                        column: x => x.TeacherID,
                        principalTable: "TEACHERS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LESSONS_CLASSES_ClassID",
                        column: x => x.ClassID,
                        principalTable: "CLASSES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTeacher",
                columns: table => new
                {
                    SubjectsID = table.Column<int>(type: "int", nullable: false),
                    TeachersID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTeacher", x => new { x.SubjectsID, x.TeachersID });
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_SUBJECTS_SubjectsID",
                        column: x => x.SubjectsID,
                        principalTable: "SUBJECTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_TEACHERS_TeachersID",
                        column: x => x.TeachersID,
                        principalTable: "TEACHERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonStudent",
                columns: table => new
                {
                    LessonsID = table.Column<int>(type: "int", nullable: false),
                    StudentsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonStudent", x => new { x.LessonsID, x.StudentsID });
                    table.ForeignKey(
                        name: "FK_LessonStudent_LESSONS_LessonsID",
                        column: x => x.LessonsID,
                        principalTable: "LESSONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonStudent_STUDENTS_StudentsID",
                        column: x => x.StudentsID,
                        principalTable: "STUDENTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Presences",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Attendance = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presences", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Presences_LESSONS_LessonID",
                        column: x => x.LessonID,
                        principalTable: "LESSONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Presences_STUDENTS_StudentID",
                        column: x => x.StudentID,
                        principalTable: "STUDENTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_ADMINISTRATORS_CPF",
                table: "ADMINISTRATORS",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ADMINISTRATORS_EMAIL",
                table: "ADMINISTRATORS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CLASSNAME_NAME",
                table: "CLASSES",
                column: "ClassName",
                unique: true,
                filter: "[ClassName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LESSONS_ClassID",
                table: "LESSONS",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_LESSONS_SubjectID",
                table: "LESSONS",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_LESSONS_TeacherID",
                table: "LESSONS",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudent_StudentsID",
                table: "LessonStudent",
                column: "StudentsID");

            migrationBuilder.CreateIndex(
                name: "IX_Presences_LessonID",
                table: "Presences",
                column: "LessonID");

            migrationBuilder.CreateIndex(
                name: "IX_Presences_StudentID",
                table: "Presences",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENTS_ClassID",
                table: "STUDENTS",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "UQ_STUDENTS_CPF",
                table: "STUDENTS",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_STUDENTS_REGISTER",
                table: "STUDENTS",
                column: "Register",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_SUBJECTNAME_NAME",
                table: "SUBJECTS",
                column: "SubjectName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeacher_TeachersID",
                table: "SubjectTeacher",
                column: "TeachersID");

            migrationBuilder.CreateIndex(
                name: "UQ_TEACHERS_CPF",
                table: "TEACHERS",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_TEACHERS_EMAIL",
                table: "TEACHERS",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADMINISTRATORS");

            migrationBuilder.DropTable(
                name: "LessonStudent");

            migrationBuilder.DropTable(
                name: "Presences");

            migrationBuilder.DropTable(
                name: "SubjectTeacher");

            migrationBuilder.DropTable(
                name: "LESSONS");

            migrationBuilder.DropTable(
                name: "STUDENTS");

            migrationBuilder.DropTable(
                name: "SUBJECTS");

            migrationBuilder.DropTable(
                name: "TEACHERS");

            migrationBuilder.DropTable(
                name: "CLASSES");
        }
    }
}
