using Microsoft.EntityFrameworkCore;

namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class retrospyContext : DbContext
    {
        public static string RetroSpyMySqlConnStr;
        public retrospyContext()
        {
        }

        public retrospyContext(DbContextOptions<retrospyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Addrequests> Addrequests { get; set; }
        public virtual DbSet<Blocked> Blocked { get; set; }
        public virtual DbSet<Friends> Friends { get; set; }
        public virtual DbSet<Games> Games { get; set; }
        public virtual DbSet<Grouplist> Grouplist { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Partner> Partner { get; set; }
        public virtual DbSet<Profiles> Profiles { get; set; }
        public virtual DbSet<Pstorage> Pstorage { get; set; }
        public virtual DbSet<Statusinfo> Statusinfo { get; set; }
        public virtual DbSet<Subprofiles> Subprofiles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(RetroSpyMySqlConnStr, x => x.ServerVersion("8.0.18-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Addrequests>(entity =>
            {
                entity.ToTable("addrequests");

                entity.HasComment("friend add request");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.HasIndex(e => e.Profileid)
                    .HasName("profileid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Namespaceid)
                    .HasColumnName("namespaceid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasColumnName("reason")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Syncrequested)
                    .IsRequired()
                    .HasColumnName("syncrequested")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Targetid)
                    .HasColumnName("targetid")
                    .HasColumnType("int(11) unsigned");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Addrequests)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_addrequests_profiles");
            });

            modelBuilder.Entity<Blocked>(entity =>
            {
                entity.ToTable("blocked");

                entity.HasComment("block list");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Namespaceid)
                    .HasColumnName("namespaceid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Targetid)
                    .HasColumnName("targetid")
                    .HasColumnType("int(11) unsigned");
            });

            modelBuilder.Entity<Friends>(entity =>
            {
                entity.ToTable("friends");

                entity.HasComment("friend list");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Namespaceid)
                    .HasColumnName("namespaceid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Targetid)
                    .HasColumnName("targetid")
                    .HasColumnType("int(11) unsigned");
            });

            modelBuilder.Entity<Games>(entity =>
            {
                entity.HasKey(e => e.Gameid)
                    .HasName("PRIMARY");

                entity.ToTable("games");

                entity.HasComment("game list that contains all secret keys of games");

                entity.HasIndex(e => e.Gameid)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Gameid)
                    .HasColumnName("gameid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Gamename)
                    .IsRequired()
                    .HasColumnName("gamename")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Queryport)
                    .HasColumnName("queryport")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'6500'");

                entity.Property(e => e.Secretkey)
                    .HasColumnName("secretkey")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Grouplist>(entity =>
            {
                entity.HasKey(e => e.Groupid)
                    .HasName("PRIMARY");

                entity.ToTable("grouplist");

                entity.HasComment("Old games use this to create their game rooms");

                entity.Property(e => e.Groupid)
                    .HasColumnName("groupid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Gameid)
                    .HasColumnName("gameid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Roomname)
                    .IsRequired()
                    .HasColumnName("roomname")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.ToTable("messages");

                entity.HasComment("friend (buddy) messages");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.From)
                    .HasColumnName("from")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Namespaceid)
                    .HasColumnName("namespaceid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.To)
                    .HasColumnName("to")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(10) unsigned");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("partner");

                entity.HasComment("partner information, these information are used for authentication and login.");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Partnerid)
                    .HasColumnName("partnerid")
                    .HasColumnType("int(10) unsigned");
            });

            modelBuilder.Entity<Profiles>(entity =>
            {
                entity.HasKey(e => e.Profileid)
                    .HasName("PRIMARY");

                entity.ToTable("profiles");

                entity.HasComment("user's profiles.");

                entity.HasIndex(e => e.Profileid)
                    .HasName("profileid")
                    .IsUnique();

                entity.HasIndex(e => e.Userid)
                    .HasName("FK_profiles_users");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Adminrights)
                    .HasColumnName("adminrights")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Aim)
                    .HasColumnName("aim")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'0'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("int(2)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Birthmonth)
                    .HasColumnName("birthmonth")
                    .HasColumnType("int(2)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Birthyear)
                    .HasColumnName("birthyear")
                    .HasColumnType("int(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Childcount)
                    .HasColumnName("childcount")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Connectiontype)
                    .HasColumnName("connectiontype")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Countrycode)
                    .IsRequired()
                    .HasColumnName("countrycode")
                    .HasColumnType("varchar(3)")
                    .HasDefaultValueSql("'1'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Cpubrandid)
                    .HasColumnName("cpubrandid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cpuspeed)
                    .HasColumnName("cpuspeed")
                    .HasColumnType("smallint(6)");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Homepage)
                    .HasColumnName("homepage")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'rspy.org'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Icquin)
                    .HasColumnName("icquin")
                    .HasColumnType("int(8) unsigned")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Incomeid)
                    .HasColumnName("incomeid")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Industryid)
                    .HasColumnName("industryid")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Interests1)
                    .HasColumnName("interests1")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("float(10,0)");

                entity.Property(e => e.Location)
                    .HasColumnName("location")
                    .HasColumnType("varchar(127)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("float(10,0)");

                entity.Property(e => e.Marriedid)
                    .HasColumnName("marriedid")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Memory)
                    .HasColumnName("memory")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Nick)
                    .IsRequired()
                    .HasColumnName("nick")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Occupationid)
                    .HasColumnName("occupationid")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Ownership1)
                    .HasColumnName("ownership1")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Picture)
                    .HasColumnName("picture")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Publicmask)
                    .HasColumnName("publicmask")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quietflags)
                    .HasColumnName("quietflags")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Serverflag)
                    .HasColumnName("serverflag")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Sex)
                    .HasColumnName("sex")
                    .HasColumnType("tinyint(1) unsigned")
                    .HasDefaultValueSql("'2'");

                entity.Property(e => e.Statstring)
                    .IsRequired()
                    .HasColumnName("statstring")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'I love RetroSpy'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Streeaddr)
                    .HasColumnName("streeaddr")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Streetaddr)
                    .HasColumnName("streetaddr")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Subscription)
                    .HasColumnName("subscription")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Videocard1ram)
                    .HasColumnName("videocard1ram")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Videocard1string)
                    .HasColumnName("videocard1string")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Videocard2ram)
                    .HasColumnName("videocard2ram")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Videocard2string)
                    .HasColumnName("videocard2string")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Zipcode)
                    .HasColumnName("zipcode")
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("'00000'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_profiles_users");
            });

            modelBuilder.Entity<Pstorage>(entity =>
            {
                entity.ToTable("pstorage");

                entity.HasComment(@"persistant storage.
old game use this to store game data.");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.HasIndex(e => e.Profileid)
                    .HasName("profileid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(4) unsigned");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasColumnType("varchar(200)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Dindex)
                    .HasColumnName("dindex")
                    .HasColumnType("int(4) unsigned");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Ptype)
                    .HasColumnName("ptype")
                    .HasColumnType("int(4) unsigned");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Pstorage)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pstorage_profiles");
            });

            modelBuilder.Entity<Statusinfo>(entity =>
            {
                entity.ToTable("statusinfo");

                entity.HasComment("Buddy Status Info table");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.HasIndex(e => e.Profileid)
                    .HasName("profileid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Buddyip)
                    .HasColumnName("buddyip")
                    .HasColumnType("varchar(16)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Gamemapname)
                    .HasColumnName("gamemapname")
                    .HasColumnType("varchar(33)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Gametype)
                    .HasColumnName("gametype")
                    .HasColumnType("varchar(33)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Gamevariant)
                    .HasColumnName("gamevariant")
                    .HasColumnType("varchar(33)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Hostip)
                    .HasColumnName("hostip")
                    .HasColumnType("varchar(16)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Hostport)
                    .HasColumnName("hostport")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Hostprivateip)
                    .HasColumnName("hostprivateip")
                    .HasColumnType("varchar(16)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Namespaceid)
                    .HasColumnName("namespaceid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Productid)
                    .HasColumnName("productid")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Queryreport)
                    .HasColumnName("queryreport")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Quietmodefalgs)
                    .IsRequired()
                    .HasColumnName("quietmodefalgs")
                    .HasColumnType("enum('NONE','MESSAGE','UTMS','LIST','ALL')")
                    .HasDefaultValueSql("'NONE'")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Richstatus)
                    .HasColumnName("richstatus")
                    .HasColumnType("varchar(256)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Sessionflags)
                    .HasColumnName("sessionflags")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Statusstate)
                    .HasColumnName("statusstate")
                    .HasColumnType("enum('OFFLINE','ONLINE','PLAYING','STAGING','CHATTING','AWAY')")
                    .HasDefaultValueSql("'OFFLINE'")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Statusinfo)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_statusinfo_profiles");
            });

            modelBuilder.Entity<Subprofiles>(entity =>
            {
                entity.ToTable("subprofiles");

                entity.HasComment("user's subprofiles");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.HasIndex(e => e.Profileid)
                    .HasName("FK_subprofiles_profiles");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Authtoken)
                    .HasColumnName("authtoken")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Cdkeyenc)
                    .HasColumnName("cdkeyenc")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Firewall)
                    .HasColumnName("firewall")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Gamename)
                    .HasColumnName("gamename")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Namespaceid)
                    .HasColumnName("namespaceid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Partnerid)
                    .HasColumnName("partnerid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Port)
                    .HasColumnName("port")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Productid)
                    .HasColumnName("productid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Uniquenick)
                    .HasColumnName("uniquenick")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Subprofiles)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_subprofiles_profiles");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("PRIMARY");

                entity.ToTable("users");

                entity.HasComment("user's account information");

                entity.HasIndex(e => e.Userid)
                    .HasName("userid")
                    .IsUnique();

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Banned).HasColumnName("banned");

                entity.Property(e => e.Createddate)
                    .HasColumnName("createddate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Emailverified)
                    .IsRequired()
                    .HasColumnName("emailverified")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Lastip)
                    .HasColumnName("lastip")
                    .HasColumnType("varchar(16)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Lastonline)
                    .HasColumnName("lastonline")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
