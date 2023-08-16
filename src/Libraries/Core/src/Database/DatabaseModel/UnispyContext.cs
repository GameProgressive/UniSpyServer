using Microsoft.EntityFrameworkCore;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.Core.Database.DatabaseModel
{
    public partial class UniSpyContext : DbContext
    {
        public UniSpyContext()
        {
        }

        public UniSpyContext(DbContextOptions<UniSpyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Addrequest> Addrequests { get; set; } = null!;
        public virtual DbSet<Blocked> Blockeds { get; set; } = null!;
        public virtual DbSet<Friend> Friends { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Grouplist> Grouplists { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Partner> Partners { get; set; } = null!;
        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<Pstorage> Pstorages { get; set; } = null!;
        public virtual DbSet<Sakestorage> Sakestorages { get; set; } = null!;
        public virtual DbSet<Subprofile> Subprofiles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConfigManager.Config.Database.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Addrequest>(entity =>
            {
                entity.ToTable("addrequests", "unispy");

                entity.HasComment("Friend request.");

                entity.Property(e => e.Addrequestid)
                    .HasColumnName("addrequestid")
                    .HasDefaultValueSql("nextval('addrequests_addrequestid_seq'::regclass)");

                entity.Property(e => e.Namespaceid).HasColumnName("namespaceid");

                entity.Property(e => e.Profileid).HasColumnName("profileid");

                entity.Property(e => e.Reason)
                    .HasColumnType("character varying")
                    .HasColumnName("reason");

                entity.Property(e => e.Syncrequested)
                    .HasColumnType("character varying")
                    .HasColumnName("syncrequested");

                entity.Property(e => e.Targetid).HasColumnName("targetid");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Addrequests)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("addrequests_fk");
            });

            modelBuilder.Entity<Blocked>(entity =>
            {
                entity.HasKey(e => e.Blockid)
                    .HasName("blocked_pk");

                entity.ToTable("blocked", "unispy");

                entity.HasComment("Block list.");

                entity.Property(e => e.Blockid)
                    .HasColumnName("blockid")
                    .HasDefaultValueSql("nextval('blocked_blockid_seq'::regclass)");

                entity.Property(e => e.Namespaceid).HasColumnName("namespaceid");

                entity.Property(e => e.Profileid).HasColumnName("profileid");

                entity.Property(e => e.Targetid).HasColumnName("targetid");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Blockeds)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("blocked_fk");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.ToTable("friends", "unispy");

                entity.HasComment("Friend list.");

                entity.Property(e => e.Friendid)
                    .HasColumnName("friendid")
                    .HasDefaultValueSql("nextval('friends_friendid_seq'::regclass)");

                entity.Property(e => e.Namespaceid).HasColumnName("namespaceid");

                entity.Property(e => e.Profileid).HasColumnName("profileid");

                entity.Property(e => e.Targetid).HasColumnName("targetid");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("friends_fk");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("games", "unispy");

                entity.HasComment("Game list.");

                entity.Property(e => e.Gameid)
                    .ValueGeneratedNever()
                    .HasColumnName("gameid");

                entity.Property(e => e.Description)
                    .HasMaxLength(4095)
                    .HasColumnName("description");

                entity.Property(e => e.Disabled).HasColumnName("disabled");

                entity.Property(e => e.Gamename)
                    .HasColumnType("character varying")
                    .HasColumnName("gamename");

                entity.Property(e => e.Secretkey)
                    .HasColumnType("character varying")
                    .HasColumnName("secretkey");
            });

            modelBuilder.Entity<Grouplist>(entity =>
            {
                entity.HasKey(e => e.Groupid)
                    .HasName("grouplist_pk");

                entity.ToTable("grouplist", "unispy");

                entity.HasComment("Old games use grouplist to create their game rooms.");

                entity.Property(e => e.Groupid)
                    .ValueGeneratedNever()
                    .HasColumnName("groupid");

                entity.Property(e => e.Gameid).HasColumnName("gameid");

                entity.Property(e => e.Roomname).HasColumnName("roomname");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Grouplists)
                    .HasForeignKey(d => d.Gameid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("grouplist_fk");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages", "unispy");

                entity.HasComment("Friend messages.");

                entity.Property(e => e.Messageid)
                    .HasColumnName("messageid")
                    .HasDefaultValueSql("nextval('messages_messageid_seq'::regclass)");

                entity.Property(e => e.Date)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.From).HasColumnName("from");

                entity.Property(e => e.Message1)
                    .HasColumnType("character varying")
                    .HasColumnName("message");

                entity.Property(e => e.Namespaceid).HasColumnName("namespaceid");

                entity.Property(e => e.To).HasColumnName("to");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("partner", "unispy");

                entity.HasComment("Partner information, these information are used for authentication and login.");

                entity.Property(e => e.Partnerid)
                    .ValueGeneratedNever()
                    .HasColumnName("partnerid");

                entity.Property(e => e.Partnername)
                    .HasColumnType("character varying")
                    .HasColumnName("partnername");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.ToTable("profiles", "unispy");

                entity.HasComment("User profiles.");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .HasDefaultValueSql("nextval('profiles_profileid_seq'::regclass)");

                entity.Property(e => e.Adminrights)
                    .HasColumnName("adminrights")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Aim)
                    .HasColumnType("character varying")
                    .HasColumnName("aim")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Birthmonth)
                    .HasColumnName("birthmonth")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Birthyear)
                    .HasColumnName("birthyear")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Childcount)
                    .HasColumnName("childcount")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.Connectiontype)
                    .HasColumnName("connectiontype")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Countrycode)
                    .HasColumnType("character varying")
                    .HasColumnName("countrycode")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Cpubrandid)
                    .HasColumnName("cpubrandid")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cpuspeed)
                    .HasColumnName("cpuspeed")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Firstname)
                    .HasColumnType("character varying")
                    .HasColumnName("firstname");

                entity.Property(e => e.Homepage)
                    .HasColumnType("character varying")
                    .HasColumnName("homepage")
                    .HasDefaultValueSql("'unispy.org'::character varying");

                entity.Property(e => e.Icquin)
                    .HasColumnName("icquin")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Incomeid)
                    .HasColumnName("incomeid")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Industryid)
                    .HasColumnName("industryid")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Interests1)
                    .HasColumnName("interests1")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Lastname)
                    .HasColumnType("character varying")
                    .HasColumnName("lastname");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Location)
                    .HasColumnType("character varying")
                    .HasColumnName("location");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Marriedid)
                    .HasColumnName("marriedid")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Memory)
                    .HasColumnName("memory")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Nick)
                    .HasColumnType("character varying")
                    .HasColumnName("nick");

                entity.Property(e => e.Occupationid)
                    .HasColumnName("occupationid")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Ownership1)
                    .HasColumnName("ownership1")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Picture)
                    .HasColumnName("picture")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Publicmask)
                    .HasColumnName("publicmask")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Quietflags).HasColumnName("quietflags");

                entity.Property(e => e.Serverflag).HasColumnName("serverflag");

                entity.Property(e => e.Sex)
                    .HasColumnName("sex")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Statstring)
                    .HasColumnType("character varying")
                    .HasColumnName("statstring")
                    .HasDefaultValueSql("'I love UniSpy'::character varying");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Streeaddr).HasColumnName("streeaddr");

                entity.Property(e => e.Streetaddr).HasColumnName("streetaddr");

                entity.Property(e => e.Subscription)
                    .HasColumnName("subscription")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Videocard1ram)
                    .HasColumnName("videocard1ram")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Videocard1string).HasColumnName("videocard1string");

                entity.Property(e => e.Videocard2ram)
                    .HasColumnName("videocard2ram")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Videocard2string).HasColumnName("videocard2string");

                entity.Property(e => e.Zipcode)
                    .HasColumnType("character varying")
                    .HasColumnName("zipcode")
                    .HasDefaultValueSql("'00000'::character varying");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profiles_fk");
            });

            modelBuilder.Entity<Pstorage>(entity =>
            {
                entity.ToTable("pstorage", "unispy");

                entity.HasComment("Old games use pstorage to store game data.");

                entity.Property(e => e.Pstorageid)
                    .HasColumnName("pstorageid")
                    .HasDefaultValueSql("nextval('pstorage_pstorageid_seq'::regclass)");

                entity.Property(e => e.Data)
                    .HasColumnType("jsonb")
                    .HasColumnName("data");

                entity.Property(e => e.Dindex).HasColumnName("dindex");

                entity.Property(e => e.Profileid).HasColumnName("profileid");

                entity.Property(e => e.Ptype).HasColumnName("ptype");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Pstorages)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pstorage_fk");
            });

            modelBuilder.Entity<Sakestorage>(entity =>
            {
                entity.ToTable("sakestorage", "unispy");

                entity.HasComment("Sake storage system.");

                entity.Property(e => e.Sakestorageid)
                    .HasColumnName("sakestorageid")
                    .HasDefaultValueSql("nextval('sakestorage_sakestorageid_seq'::regclass)");

                entity.Property(e => e.Profileid).HasColumnName("profileid");

                entity.Property(e => e.Tableid)
                    .HasColumnType("character varying")
                    .HasColumnName("tableid");

                entity.Property(e => e.Userdata)
                    .HasColumnType("jsonb")
                    .HasColumnName("userdata");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Sakestorages)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profileid_fk");
            });

            modelBuilder.Entity<Subprofile>(entity =>
            {
                entity.ToTable("subprofiles", "unispy");

                entity.HasComment("User subprofiles.");

                entity.Property(e => e.Subprofileid)
                    .HasColumnName("subprofileid")
                    .HasDefaultValueSql("nextval('subprofiles_subprofileid_seq'::regclass)");

                entity.Property(e => e.Authtoken)
                    .HasColumnType("character varying")
                    .HasColumnName("authtoken");

                entity.Property(e => e.Cdkeyenc)
                    .HasColumnType("character varying")
                    .HasColumnName("cdkeyenc");

                entity.Property(e => e.Firewall)
                    .HasColumnName("firewall")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Gamename).HasColumnName("gamename");

                entity.Property(e => e.Namespaceid).HasColumnName("namespaceid");

                entity.Property(e => e.Partnerid).HasColumnName("partnerid");

                entity.Property(e => e.Port)
                    .HasColumnName("port")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Profileid).HasColumnName("profileid");

                entity.Property(e => e.Uniquenick)
                    .HasColumnType("character varying")
                    .HasColumnName("uniquenick");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Subprofiles)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("subprofiles_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "unispy");

                entity.HasComment("User account information.");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasDefaultValueSql("nextval('users_userid_seq'::regclass)");

                entity.Property(e => e.Banned).HasColumnName("banned");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createddate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Email)
                    .HasColumnType("character varying")
                    .HasColumnName("email");

                entity.Property(e => e.Emailverified)
                    .IsRequired()
                    .HasColumnName("emailverified")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Lastip).HasColumnName("lastip");

                entity.Property(e => e.Lastonline)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("lastonline")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Password)
                    .HasColumnType("character varying")
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
