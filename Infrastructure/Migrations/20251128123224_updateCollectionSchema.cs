using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCollectionSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionTag_Collections_CollectionId",
                table: "CollectionTag");

            migrationBuilder.DropForeignKey(
                name: "FK_CollectionTag_Tags_TagsId",
                table: "CollectionTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CollectionTag",
                table: "CollectionTag");

            migrationBuilder.RenameTable(
                name: "CollectionTag",
                newName: "TagCollections");

            migrationBuilder.RenameIndex(
                name: "IX_CollectionTag_TagsId",
                table: "TagCollections",
                newName: "IX_TagCollections_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagCollections",
                table: "TagCollections",
                columns: new[] { "CollectionId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TagCollections_Collections_CollectionId",
                table: "TagCollections",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagCollections_Tags_TagsId",
                table: "TagCollections",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagCollections_Collections_CollectionId",
                table: "TagCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_TagCollections_Tags_TagsId",
                table: "TagCollections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagCollections",
                table: "TagCollections");

            migrationBuilder.RenameTable(
                name: "TagCollections",
                newName: "CollectionTag");

            migrationBuilder.RenameIndex(
                name: "IX_TagCollections_TagsId",
                table: "CollectionTag",
                newName: "IX_CollectionTag_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CollectionTag",
                table: "CollectionTag",
                columns: new[] { "CollectionId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionTag_Collections_CollectionId",
                table: "CollectionTag",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionTag_Tags_TagsId",
                table: "CollectionTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
