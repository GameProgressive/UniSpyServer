--
-- PostgreSQL database dump
--

-- Dumped from database version 14.2 (Debian 14.2-1.pgdg110+1)
-- Dumped by pg_dump version 14.1

-- Started on 2022-02-27 02:04:35 CET

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5 (class 2615 OID 16385)
-- Name: unispy; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA unispy;


--
-- TOC entry 3466 (class 0 OID 0)
-- Dependencies: 5
-- Name: SCHEMA unispy; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA unispy IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 209 (class 1259 OID 16386)
-- Name: addrequests; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.addrequests (
    addrequestid integer NOT NULL,
    profileid integer NOT NULL,
    namespaceid integer NOT NULL,
    targetid integer NOT NULL,
    reason character varying NOT NULL,
    syncrequested character varying NOT NULL
);


--
-- TOC entry 3467 (class 0 OID 0)
-- Dependencies: 209
-- Name: TABLE addrequests; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.addrequests IS 'Friend request.';


--
-- TOC entry 210 (class 1259 OID 16391)
-- Name: addrequests_addrequestid_seq; Type: SEQUENCE; Schema: unispy; Owner: -
--

CREATE SEQUENCE unispy.addrequests_addrequestid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3468 (class 0 OID 0)
-- Dependencies: 210
-- Name: addrequests_addrequestid_seq; Type: SEQUENCE OWNED BY; Schema: unispy; Owner: -
--

ALTER SEQUENCE unispy.addrequests_addrequestid_seq OWNED BY unispy.addrequests.addrequestid;


--
-- TOC entry 211 (class 1259 OID 16392)
-- Name: blocked; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.blocked (
    blockid integer NOT NULL,
    profileid integer NOT NULL,
    namespaceid integer NOT NULL,
    targetid integer NOT NULL
);


--
-- TOC entry 3469 (class 0 OID 0)
-- Dependencies: 211
-- Name: TABLE blocked; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.blocked IS 'Block list.';


--
-- TOC entry 212 (class 1259 OID 16395)
-- Name: blocked_blockid_seq; Type: SEQUENCE; Schema: unispy; Owner: -
--

CREATE SEQUENCE unispy.blocked_blockid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3470 (class 0 OID 0)
-- Dependencies: 212
-- Name: blocked_blockid_seq; Type: SEQUENCE OWNED BY; Schema: unispy; Owner: -
--

ALTER SEQUENCE unispy.blocked_blockid_seq OWNED BY unispy.blocked.blockid;


--
-- TOC entry 213 (class 1259 OID 16396)
-- Name: friends; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.friends (
    friendid integer NOT NULL,
    profileid integer NOT NULL,
    namespaceid integer NOT NULL,
    targetid integer NOT NULL
);


--
-- TOC entry 3471 (class 0 OID 0)
-- Dependencies: 213
-- Name: TABLE friends; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.friends IS 'Friend list.';


--
-- TOC entry 214 (class 1259 OID 16399)
-- Name: friends_friendid_seq; Type: SEQUENCE; Schema: unispy; Owner: -
--

CREATE SEQUENCE unispy.friends_friendid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3472 (class 0 OID 0)
-- Dependencies: 214
-- Name: friends_friendid_seq; Type: SEQUENCE OWNED BY; Schema: unispy; Owner: -
--

ALTER SEQUENCE unispy.friends_friendid_seq OWNED BY unispy.friends.friendid;


--
-- TOC entry 215 (class 1259 OID 16400)
-- Name: games; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.games (
    gameid integer NOT NULL,
    gamename character varying NOT NULL,
    secretkey character varying,
    description character varying(4095) NOT NULL,
    disabled boolean NOT NULL
);


--
-- TOC entry 3473 (class 0 OID 0)
-- Dependencies: 215
-- Name: TABLE games; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.games IS 'Game list.';


--
-- TOC entry 216 (class 1259 OID 16405)
-- Name: grouplist; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.grouplist (
    groupid integer NOT NULL,
    gameid integer NOT NULL,
    roomname text NOT NULL
);


--
-- TOC entry 3474 (class 0 OID 0)
-- Dependencies: 216
-- Name: TABLE grouplist; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.grouplist IS 'Old games use grouplist to create their game rooms.';


--
-- TOC entry 217 (class 1259 OID 16410)
-- Name: messages; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.messages (
    messageid integer NOT NULL,
    namespaceid integer,
    type integer,
    "from" integer NOT NULL,
    "to" integer NOT NULL,
    date timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    message character varying NOT NULL
);


--
-- TOC entry 3475 (class 0 OID 0)
-- Dependencies: 217
-- Name: TABLE messages; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.messages IS 'Friend messages.';


--
-- TOC entry 218 (class 1259 OID 16416)
-- Name: messages_messageid_seq; Type: SEQUENCE; Schema: unispy; Owner: -
--

CREATE SEQUENCE unispy.messages_messageid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3476 (class 0 OID 0)
-- Dependencies: 218
-- Name: messages_messageid_seq; Type: SEQUENCE OWNED BY; Schema: unispy; Owner: -
--

ALTER SEQUENCE unispy.messages_messageid_seq OWNED BY unispy.messages.messageid;


--
-- TOC entry 219 (class 1259 OID 16417)
-- Name: partner; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.partner (
    partnerid integer NOT NULL,
    partnername character varying NOT NULL
);


--
-- TOC entry 3477 (class 0 OID 0)
-- Dependencies: 219
-- Name: TABLE partner; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.partner IS 'Partner information, these information are used for authentication and login.';


--
-- TOC entry 220 (class 1259 OID 16422)
-- Name: profiles; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.profiles (
    profileid integer NOT NULL,
    userid integer NOT NULL,
    nick character varying NOT NULL,
    serverflag integer DEFAULT 0 NOT NULL,
    status smallint DEFAULT 0,
    statstring character varying DEFAULT 'I love UniSpy'::character varying,
    location character varying,
    firstname character varying,
    lastname character varying,
    publicmask integer DEFAULT 0,
    latitude double precision DEFAULT 0,
    longitude double precision DEFAULT 0,
    aim character varying DEFAULT ''::character varying,
    picture integer DEFAULT 0,
    occupationid integer DEFAULT 0,
    incomeid integer DEFAULT 0,
    industryid integer DEFAULT 0,
    marriedid integer DEFAULT 0,
    childcount integer DEFAULT 0,
    interests1 integer DEFAULT 0,
    ownership1 integer DEFAULT 0,
    connectiontype integer DEFAULT 0,
    sex smallint DEFAULT 0,
    zipcode character varying DEFAULT '00000'::character varying,
    countrycode character varying DEFAULT 1,
    homepage character varying DEFAULT 'unispy.org'::character varying,
    birthday integer DEFAULT 0,
    birthmonth integer DEFAULT 0,
    birthyear integer DEFAULT 0,
    icquin integer DEFAULT 0,
    quietflags smallint DEFAULT 0 NOT NULL,
    streetaddr text,
    streeaddr text,
    city text,
    cpubrandid integer DEFAULT 0,
    cpuspeed integer DEFAULT 0,
    memory smallint DEFAULT 0,
    videocard1string text,
    videocard1ram smallint DEFAULT 0,
    videocard2string text,
    videocard2ram smallint DEFAULT 0,
    subscription integer DEFAULT 0,
    adminrights integer DEFAULT 0
);


--
-- TOC entry 3478 (class 0 OID 0)
-- Dependencies: 220
-- Name: TABLE profiles; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.profiles IS 'User profiles.';


--
-- TOC entry 221 (class 1259 OID 16459)
-- Name: profiles_profileid_seq; Type: SEQUENCE; Schema: unispy; Owner: -
--

CREATE SEQUENCE unispy.profiles_profileid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3479 (class 0 OID 0)
-- Dependencies: 221
-- Name: profiles_profileid_seq; Type: SEQUENCE OWNED BY; Schema: unispy; Owner: -
--

ALTER SEQUENCE unispy.profiles_profileid_seq OWNED BY unispy.profiles.profileid;


--
-- TOC entry 222 (class 1259 OID 16460)
-- Name: pstorage; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.pstorage (
    pstorageid integer NOT NULL,
    profileid integer NOT NULL,
    ptype integer NOT NULL,
    dindex integer NOT NULL,
    data jsonb
);


--
-- TOC entry 3480 (class 0 OID 0)
-- Dependencies: 222
-- Name: TABLE pstorage; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.pstorage IS 'Old games use pstorage to store game data.';


--
-- TOC entry 223 (class 1259 OID 16465)
-- Name: pstorage_pstorageid_seq; Type: SEQUENCE; Schema: unispy; Owner: -
--

CREATE SEQUENCE unispy.pstorage_pstorageid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3481 (class 0 OID 0)
-- Dependencies: 223
-- Name: pstorage_pstorageid_seq; Type: SEQUENCE OWNED BY; Schema: unispy; Owner: -
--

ALTER SEQUENCE unispy.pstorage_pstorageid_seq OWNED BY unispy.pstorage.pstorageid;


--
-- TOC entry 224 (class 1259 OID 16466)
-- Name: sakestorage; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.sakestorage (
    sakestorageid integer NOT NULL,
    tableid character varying NOT NULL
);


--
-- TOC entry 3482 (class 0 OID 0)
-- Dependencies: 224
-- Name: TABLE sakestorage; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.sakestorage IS 'Sake storage system.';


--
-- TOC entry 225 (class 1259 OID 16471)
-- Name: sakestorage_sakestorageid_seq; Type: SEQUENCE; Schema: unispy; Owner: -
--

CREATE SEQUENCE unispy.sakestorage_sakestorageid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3483 (class 0 OID 0)
-- Dependencies: 225
-- Name: sakestorage_sakestorageid_seq; Type: SEQUENCE OWNED BY; Schema: unispy; Owner: -
--

ALTER SEQUENCE unispy.sakestorage_sakestorageid_seq OWNED BY unispy.sakestorage.sakestorageid;


--
-- TOC entry 226 (class 1259 OID 16472)
-- Name: subprofiles; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.subprofiles (
    subprofileid integer NOT NULL,
    profileid integer NOT NULL,
    uniquenick character varying,
    namespaceid integer DEFAULT 0 NOT NULL,
    partnerid integer DEFAULT 0 NOT NULL,
    productid integer,
    gamename text,
    cdkeyenc character varying,
    firewall smallint DEFAULT 0,
    port integer DEFAULT 0,
    authtoken character varying
);


--
-- TOC entry 3484 (class 0 OID 0)
-- Dependencies: 226
-- Name: TABLE subprofiles; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.subprofiles IS 'User subprofiles.';


--
-- TOC entry 227 (class 1259 OID 16481)
-- Name: subprofiles_subprofileid_seq; Type: SEQUENCE; Schema: unispy; Owner: -
--

CREATE SEQUENCE unispy.subprofiles_subprofileid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3485 (class 0 OID 0)
-- Dependencies: 227
-- Name: subprofiles_subprofileid_seq; Type: SEQUENCE OWNED BY; Schema: unispy; Owner: -
--

ALTER SEQUENCE unispy.subprofiles_subprofileid_seq OWNED BY unispy.subprofiles.subprofileid;


--
-- TOC entry 228 (class 1259 OID 16482)
-- Name: users; Type: TABLE; Schema: unispy; Owner: -
--

CREATE TABLE unispy.users (
    userid integer NOT NULL,
    email character varying NOT NULL,
    password character varying NOT NULL,
    emailverified boolean DEFAULT true NOT NULL,
    lastip inet,
    lastonline timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    createddate timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    banned boolean DEFAULT false NOT NULL,
    deleted boolean DEFAULT false NOT NULL
);


--
-- TOC entry 3486 (class 0 OID 0)
-- Dependencies: 228
-- Name: TABLE users; Type: COMMENT; Schema: unispy; Owner: -
--

COMMENT ON TABLE unispy.users IS 'User account information.';


--
-- TOC entry 229 (class 1259 OID 16492)
-- Name: users_userid_seq; Type: SEQUENCE; Schema: unispy; Owner: -
--

CREATE SEQUENCE unispy.users_userid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3487 (class 0 OID 0)
-- Dependencies: 229
-- Name: users_userid_seq; Type: SEQUENCE OWNED BY; Schema: unispy; Owner: -
--

ALTER SEQUENCE unispy.users_userid_seq OWNED BY unispy.users.userid;


--
-- TOC entry 3219 (class 2604 OID 16493)
-- Name: addrequests addrequestid; Type: DEFAULT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.addrequests ALTER COLUMN addrequestid SET DEFAULT nextval('unispy.addrequests_addrequestid_seq'::regclass);


--
-- TOC entry 3220 (class 2604 OID 16494)
-- Name: blocked blockid; Type: DEFAULT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.blocked ALTER COLUMN blockid SET DEFAULT nextval('unispy.blocked_blockid_seq'::regclass);


--
-- TOC entry 3221 (class 2604 OID 16495)
-- Name: friends friendid; Type: DEFAULT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.friends ALTER COLUMN friendid SET DEFAULT nextval('unispy.friends_friendid_seq'::regclass);


--
-- TOC entry 3223 (class 2604 OID 16496)
-- Name: messages messageid; Type: DEFAULT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.messages ALTER COLUMN messageid SET DEFAULT nextval('unispy.messages_messageid_seq'::regclass);


--
-- TOC entry 3256 (class 2604 OID 16497)
-- Name: profiles profileid; Type: DEFAULT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.profiles ALTER COLUMN profileid SET DEFAULT nextval('unispy.profiles_profileid_seq'::regclass);


--
-- TOC entry 3257 (class 2604 OID 16498)
-- Name: pstorage pstorageid; Type: DEFAULT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.pstorage ALTER COLUMN pstorageid SET DEFAULT nextval('unispy.pstorage_pstorageid_seq'::regclass);


--
-- TOC entry 3258 (class 2604 OID 16499)
-- Name: sakestorage sakestorageid; Type: DEFAULT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.sakestorage ALTER COLUMN sakestorageid SET DEFAULT nextval('unispy.sakestorage_sakestorageid_seq'::regclass);


--
-- TOC entry 3263 (class 2604 OID 16500)
-- Name: subprofiles subprofileid; Type: DEFAULT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.subprofiles ALTER COLUMN subprofileid SET DEFAULT nextval('unispy.subprofiles_subprofileid_seq'::regclass);


--
-- TOC entry 3269 (class 2604 OID 16501)
-- Name: users userid; Type: DEFAULT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.users ALTER COLUMN userid SET DEFAULT nextval('unispy.users_userid_seq'::regclass);


--
-- TOC entry 3440 (class 0 OID 16386)
-- Dependencies: 209
-- Data for Name: addrequests; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.addrequests (addrequestid, profileid, namespaceid, targetid, reason, syncrequested) FROM stdin;
\.


--
-- TOC entry 3442 (class 0 OID 16392)
-- Dependencies: 211
-- Data for Name: blocked; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.blocked (blockid, profileid, namespaceid, targetid) FROM stdin;
\.


--
-- TOC entry 3444 (class 0 OID 16396)
-- Dependencies: 213
-- Data for Name: friends; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.friends (friendid, profileid, namespaceid, targetid) FROM stdin;
\.


--
-- TOC entry 3446 (class 0 OID 16400)
-- Dependencies: 215
-- Data for Name: games; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.games (gameid, gamename, secretkey, description, disabled) FROM stdin;
1	gmtest	HA6zkS	Test / demo / temporary	f
2	bgate	\N	Baldur's Gate	f
3	blood2	\N	Blood II	f
4	bz2	\N	Battlezone 2	f
5	daikatana	\N	Daikatana	f
6	descent3	\N	Descent 3	f
7	dh3	\N	Deer Hunter 3	f
8	drakan	\N	Drakan	f
9	dv	\N	Dark Vengeance	f
10	expertpool	\N	Expert Pool	f
11	forsaken	\N	Forsaken	f
12	gamespy2	\N	GameSpy 3D	f
13	gspylite	\N	GameSpy Lite	f
14	gspyweb	\N	GameSpy Web	f
15	halflife	\N	Half Life	f
16	heretic2	\N	Heretic II	f
17	hexenworld	\N	Hexenworld	f
18	kingpin	\N	Kingpin	f
19	mplayer	\N	MPlayer	f
20	quake1	\N	Quake	f
21	quake2	\N	Quake II	f
22	quake3	\N	Quake 3: Arena	f
23	quakeworld	\N	Quakeworld	f
24	rally	\N	Rally Masters	f
25	redline	\N	Redline	f
26	shogo	\N	Shogo	f
27	sin	\N	SiN	f
28	slavezero	\N	Slave Zero	f
29	sof	\N	Soldier of Fortune	f
30	southpark	\N	South Park	f
31	specops	\N	Spec Ops	f
32	tribes	\N	Starsiege TRIBES	f
33	turok2	\N	Turok 2	f
34	unreal	\N	Unreal	f
35	ut	\N	Unreal Tournament	f
36	viper	\N	Viper	f
39	nerfarena	\N	Nerf Arena	f
40	wot	\N	Wheel of Time	f
41	giants	\N	Giants	f
42	dtracing	\N	Dirt Track Racing	f
43	terminus	\N	Terminus	f
44	darkreign2	\N	Dark Reign 2	f
45	ra2	\N	Rocket Arena 2	f
46	aoe2	\N	Age of Empires 2	f
47	roguespear	\N	Rogue Spear	f
48	scompany	\N	Shadow Company	f
49	scrabble	\N	Scrabble	f
50	boggle	\N	Boggle	f
51	werewolf	\N	Werewolf: The Apocalypse	f
52	treadmarks	\N	Tread Marks	f
53	avp	\N	Aliens Vs Predator	f
54	rock	\N	Rock	f
55	midmad	\N	Midtown Madness	f
56	aoe	\N	Age of Empires	f
57	revolt	\N	Revolt	f
58	gslive	\N	GameSpy Arcade	f
60	paintball	\N	Paintball	f
61	wildwings	\N	Wild Wings	f
62	rmth3	\N	Rocky Mountain Trophy Hunter 3	f
64	metalcrush3	\N	Metal Crush 3	f
65	ta	\N	Total Annihilation	f
67	mech3	\N	Mech Warrior 3	f
68	railty2	\N	Railroad Tycoon II	f
70	mcmad	\N	Motocross Madness	f
71	heroes3	\N	Heroes Of Might And Magic III	f
72	jk	\N	Star Wars: Jedi Knight	f
73	links98	\N	Links LS 1998	f
75	irl2000	\N	Indy Racing League 2000	f
84	xwingtie	\N	Star Wars: X-Wing vs. TIE Fighter	f
99	buckmaster	\N	Buckmaster Deer Hunting	f
100	cneagle	\N	Codename: Eagle	f
104	alphacent	\N	Sid Meier's Alpha Centauri	f
108	sanity	\N	Sanity	f
112	jetfighter4	\N	Jet Fighter 4	f
113	starraiders	\N	Star Raiders	f
114	kiss	\N	KISS: Psycho Circus	f
121	risk	\N	Risk C.1997	f
122	cribbage	\N	Hasbro's Cribbage	f
123	populoustb	\N	Populous: The Beginning	f
125	ginrummy	\N	Hasbro's Gin Rummy	f
126	hearts	\N	Hasbro's Hearts	f
128	spades	\N	Hasbro's Spades	f
129	racko	\N	Hasbro's Racko	f
130	rook	\N	Hasbro's Rook	f
131	checkers	\N	Hasbro's Checkers	f
133	chess	\N	Hasbro's Chess	f
134	dominos	\N	Hasbro's Dominos	f
135	tzar	\N	TZAR	f
136	parcheesi	\N	Hasbro's Parcheesi	f
137	pente	\N	Hasbro's Pente	f
138	backgammon	\N	Hasbro's Backgammon	f
139	freepark	\N	Hasbro's Free Parking	f
140	connect4	\N	Hasbro's Connect 4	f
141	millebourn	\N	Hasbro's Mille Bournes	f
142	msgolf99	\N	Microsoft Golf 99	f
143	civ2tot	\N	Civilization II: Test of Time	f
144	close4bb	\N	CloseCombat4BB	f
145	aliencross	\N	Alien Crossfire	f
146	outlaws	\N	Outlaws	f
147	civ2gold	\N	Civilization 2: Gold	f
148	getmede	\N	Get Medieval	f
149	gruntz	\N	Gruntz	f
150	monopoly	\N	Monopoly	f
151	rb6	\N	Rainbow Six	f
152	wz2100	\N	Warzone 2100	f
153	rebellion	\N	Star Wars: Rebellion	f
154	ccombat3	\N	Close Combat 3	f
155	baldursg	\N	Baludurs Gate	f
156	jkmosith1	\N	Star Wars Jedi Knight: Mysteries of the Sith1	f
157	smgettysbu	\N	Sid Meier's Gettysburg	f
158	srally2dmo	\N	Sega Rally 2 PC Demo	f
159	fltsim2k	\N	Flight Simulator 2000	f
160	aowdemo	\N	Age Of Wonders Demo	f
161	duke4	\N	Duke Nukem Forever	f
162	aowfull	\N	Age Of Wonders	f
163	darkstone	\N	Darkstone	f
164	abominatio	\N	Abomination	f
165	bc3k	\N	Battle Cruiser 3000 AD	f
166	outlawsdem	\N	Outlaw Multiplay Demo	f
167	allegiance	\N	MS Allegiance	f
168	rsurbanops	\N	Rogue Spear: Urban Ops	f
169	aoe2demo	\N	Age of Empires II Demo	f
170	mcmaddemo	\N	Motocross Madness Demo	f
171	midmaddemo	\N	Midtown Madness Demo	f
172	mtmdemo	\N	Monster Truck Madness Demo	f
173	axisallies	\N	Axis & Allies	f
174	rallychamp	\N	Mobil1 Rally Championship	f
175	worms2	\N	Worms 2	f
176	mtruckm2	\N	Monster Truck Madness 2	f
177	powerslide	\N	Powerslide	f
178	kissdc	\N	Kiss (Dreamcast)	f
179	legendsmm	\N	Legends of Might and Magic	f
180	mech4	\N	Mechwarrior 4	f
181	sofretail	\N	Soldier of Fortune: Retail	f
182	majesty	\N	Majesty	f
183	fblackjack	\N	Fiendish Blackjack	f
184	slancerdc	\N	Starlancer (Dreamcast)	f
185	fbackgammon	\N	Fiendish Backgammon	f
186	dogsofwar	\N	Dogs of War	f
187	starlancer	\N	Starlancer	f
188	laserarena	\N	Laser Arena (2015)	f
189	mmadness2	\N	Motocross Madness 2	f
190	obiwon	\N	Obi-Wan	f
191	ra3	\N	Rocket Arena 3	f
192	virtualpool3	\N	Virtual Pool 3	f
195	sanitydemo	\N	Sanity Demo	f
196	sanitybeta	\N	Sanity Beta	f
197	Frogger	\N	Frogger	f
198	stitandemo	\N	Submarine Titans Demo	f
199	stbotf	\N	Birth of the Federation	f
200	machines	\N	Machines	f
201	amairtac	\N	Army Men - Air Tactics	f
202	amworldwar	\N	Army Men World War	f
203	gettysburg	\N	Gettysburg	f
204	hhbball2000	\N	High Heat Baseball 2000	f
205	dogalo	\N	MechWarrior 3	f
206	armymen2	\N	Army Men II	f
207	armymenspc	\N	Army Men Toys in Space	f
208	hhbball2001	\N	High Heat Baseball 2001	f
209	risk2	\N	Risk II	f
210	starwrsfrc	\N	Star Wars: Force Commander	f
211	peoplesgen	\N	Peoples General	f
212	planecrazy	\N	Plane Crazy	f
213	linksext	\N	Links Extreme	f
214	flyinghero	\N	Flying Heroes	f
215	fltsim98	\N	Microsoft Flight Simulator 98	f
216	links2000	\N	Links LS 2000	f
217	ritesofwar	\N	Warhammer: Rites of War	f
218	gulfwarham	\N	Gulf War: Operatin Desert	f
219	uprising2	\N	Uprising 2	f
220	earth2150	\N	Earth 2150	f
221	evolva	\N	Evolva	f
222	eawar	\N	European Air War	f
223	7kingdoms	\N	Seven Kingdoms 2	f
224	migalley	\N	Mig Alley	f
225	axallirnb	\N	Axis & Allies: Iron Blitz	f
226	rrt2scnd	\N	Railroad Tycoon 2: The Second Century	f
227	mcommgold	\N	MechCommander Gold	f
228	santietam	\N	Sid Meier's Antietam!	f
229	heroes3arm	\N	Heroes of Might and Magic	f
230	panzergen2	\N	Panzer General	f
231	lazgo2demo	\N	Lazgo 2 Demo	f
232	taking	\N	Total Annihilation: Kingdoms	f
233	mfatigue	\N	Metal Fatigue	f
234	cfs	\N	Microsoft Combat Flight Simulator	f
235	starsiege	\N	Starsiege	f
236	jkmots	\N	Star Wars Jedi Knight: Mysteries of the Sith	f
237	zdoom	\N	ZDoom	f
238	warlordsb	\N	Warlords Battlecry	f
239	bangdemo	\N	Bang! Gunship Elite Demo	f
240	anno1602ad	\N	Anno 1602 A.D.	f
241	dh4	\N	Deer Hunter 4	f
242	group	\N	Group Room	f
243	blademasters	\N	Legend of the Blademasters	f
244	iwdale	\N	Icewind Dale	f
245	dogsrunamock	\N	dogsrunamock (?)	f
246	excessive	\N	Excessive Q3	f
247	bgate2	\N	Baldur's Gate II: Shadows of Amn	f
248	mcm2demo	\N	Motocross Madness 2 Demo	f
249	dtrsc	\N	Dirt Track Racing: Sprint Cars	f
250	chspades	\N	Championship Spades	f
251	chhearts	\N	Championship Hearts	f
252	stef1	\N	Star Trek: Voyager – Elite Force	f
253	orb	\N	O.R.B: Off-World Resource Base	f
254	nolf	\N	No One Lives Forever	f
255	dtr	\N	Dirt Track Racing	f
256	sacrifice	\N	Sacrifice	f
257	rune	\N	Rune	f
258	aoe2tc	\N	Age of Empires II: The Conquerors	f
259	stitans	\N	Submarine Titans	f
260	bang	\N	Bang! Gunship Elite	f
261	aoe2tcdemo	\N	Age of Empires II: The Conquerors Demo	f
262	fakk2	\N	F.A.K.K. 2	f
263	bcm	\N	Battlecruiser: Millenium	f
264	ds9dominion	\N	DS9: Dominion Wars	f
265	bots	\N	Bots (Lith)	f
266	tacore	\N	Core Contingency	f
267	mech3pm	\N	Pirates Moon	f
268	diplomacy	\N	Diplomacy	f
269	bandw	\N	Black and White	f
270	fargate	\N	Far Gate	f
271	nexttetris	\N	The Next Tetris	f
272	fforce	\N	Freedom Force	f
273	iwar2	\N	Independance War 2	f
274	gp500	\N	GP500	f
275	midmad2	\N	Midtown Madness 2	f
276	insane	\N	Insane	f
277	unreal2	\N	Unreal 2	f
278	4x4evo	\N	4x4 Evolution	f
279	crimson	\N	Crimson Skies	f
280	harleywof	\N	Wheels of Freedom	f
281	dtrscdmo	\N	Dirt Track Racing: Sprint	f
282	ageofsail2	\N	Age of Sail 2	f
283	cskies	\N	Crimson Skies	f
284	rscovertops	\N	Rainbow Six: Covert Ops	f
285	pba2001	\N	PBA Bowling 2001	f
286	cskiesdemo	\N	Crimson Skies Demo	f
287	mech4st	\N	MechWarrior 4: Vengeance	f
288	wosin	\N	SiN: Wages of Sin	f
289	sinmac	\N	SiN (Mac)	f
290	wosinmac	\N	SiN: Wages of Sin (Mac)	f
291	utdc	\N	Unreal Tournament (Dreamcast)	f
292	kohan	\N	Kohan: Immortal Sovereigns	f
293	mcmania	\N	Motocross Mania	f
294	close5	\N	Close Combat 5	f
295	furfiighters	\N	Fur Fighters (?)	f
296	furfighters	\N	Fur Fighters	f
297	owar	\N	Original War	f
298	cfs2	\N	Combat Flight Simulator 2	f
299	uno	\N	UNO	f
301	deusex	\N	Deus Ex	f
302	gore	\N	Gore	f
303	gangsters2	\N	Gansters II: Vendetta	f
304	insanedmo	\N	Insane Demo	f
305	close5dmo	\N	Close Combat 5 Demo	f
306	atlantis	\N	Atlantis	f
307	cossacks	p2vPkJ	Cossacks Anthology	f
308	ihraracing	\N	IHRA Drag Racing	f
309	atlantispre	\N	Atlantis Prequel	f
310	4x4retail	\N	4x4 Evolution	f
311	rnconsole	\N	Real Networks Console	f
312	dukes	\N	Dukes Of Hazzard: Racing	f
313	serioussam	\N	Serious Sam	f
314	runedemo	\N	Rune Demo	f
315	suddenstrike	\N	Sudden Strike	f
316	rfts	\N	Reach For The Stars	f
317	cheuchre	\N	Championship Euchre	f
318	links2001	\N	Links 2001	f
319	stefdemo	\N	Star Trek: Voyager – Elite Force Demo	f
320	4x4evodemo	\N	4x4 Evolution Demo	f
321	mcmaniadmo	\N	Motocross Mania Demo	f
322	gamevoice	\N	MS Game Voice	f
323	cstrike	\N	Counter-Strike	f
324	venomworld	\N	Venom World	f
325	omfbattle	\N	One Must Fall Battlegrounds	f
326	furdemo	\N	Fur Fighters Demo	f
327	q3tademo	\N	Team Arena Demo	f
328	nwn	\N	Neverwinter Nights	f
329	strifeshadow	\N	Strifeshadow	f
330	ssamdemo	\N	Serious Sam Demo	f
331	kacademy	\N	Klingon Academy	f
332	goredemo	\N	Gore Demo	f
333	majestyx	\N	Majesty Expansion	f
334	midmad2dmo	\N	Midtown Madness 2 Demo	f
335	gunman	\N	Gunman Chronicles	f
336	stronghold	\N	Stronghold	f
337	armada2	N3a2mZ	Star Trek Armada 2	f
338	links2001dmo	\N	Links 2001 Demo	f
339	q3tafull	\N	Team Arena Retail	f
340	botbattles	\N	Tex Atomics Big Bot Battles	f
341	battlerealms	\N	Battle Realms	f
342	sfc	\N	Starfleet Command	f
343	strfltcmd2	\N	Starfleet Command Volume	f
344	stnw	\N	Star Trek: New Worlds	f
345	strfltcmd2d	\N	Empires at War Demo	f
346	sfcdemo	\N	Starfleet Command Demo	f
347	crmgdntdr2k	\N	Carmageddon TDR 2000	f
348	waterloo	\N	Waterloo	f
349	falloutbosd	\N	Fallout Tactics	f
350	kohandemo	\N	Kohan Demo	f
351	exploeman	\N	Explöman	f
352	segarally2	\N	Sega Rally 2	f
353	explomän	\N	Explomän	f
354	streetjam	\N	Ultra Wheels Street Jam	f
355	explomaen	\N	Explomän	f
356	bcommander	\N	Star Trek: Bridge Commander	f
357	mrwtour	\N	Motoracer World Tour	f
358	wordzap	\N	WordZap	f
359	iwdalehow	\N	Icewind Dale: Heart of Winter	f
360	magmay2	\N	Magic & Mayhem 2	f
361	chat01	\N	Chat Group 1	f
362	chat02	\N	Chat Group 2	f
363	chat03	\N	Chat Group 3	f
364	Chat04	\N	Chat Group 4	f
365	Chat05	\N	Chat Group 5	f
366	Chat06	\N	Chat Group 6	f
367	Chat07	\N	Chat Group 7	f
368	Chat08	\N	Chat Group 8	f
369	Chat09	\N	Chat Group 9	f
370	Chat10	\N	Chat Group 10	f
371	Chat11	\N	Chat Group 11	f
372	Chat12	\N	Chat Group 12	f
373	Chat13	\N	Chat Group 13	f
374	Chat14	\N	Chat Group 14	f
375	Chat15	\N	Chat Group 15	f
376	Chat16	\N	Chat Group 16	f
377	Chat17	\N	Chat Group 17	f
378	Chat18	\N	Chat Group 18	f
379	Chat19	\N	Chat Group 19	f
380	Chat20	\N	Chat Group 20	f
381	empireearth	\N	Empire Earth	f
382	chasspart5	\N	ChessPartner 5	f
383	bg2bhaal	\N	Baldur's Gate II: Throne of Bhaal	f
384	legendsmmbeta	\N	Legends of Might and Magic Beta	f
385	cultures	\N	Cultures	f
386	fatedragon	\N	Fate of the Dragon	f
387	sbubpop	\N	Super Bubble Pop	f
388	xcomenforcer	\N	X-Com: Enforcer	f
389	aow2	\N	Age of Wonders 2	f
390	startopia	\N	Startopia	f
391	jefftest	\N	Test for Jeffs Games	f
392	hhbball2002	\N	High Heat Baseball 2002	f
394	por2	\N	Pool of Radiance 2	f
395	falloutbos	\N	Fallout Tactics	f
396	moonproject	\N	Moon Project	f
397	fatedragond	\N	Fate of the Dragon Demo	f
398	demonstar	\N	Demonstar	f
399	tf15	\N	Half-Life 1.5	f
400	gspoker	\N	GameSpy Poker	f
401	gsspades	\N	GameSpy Spades	f
402	gshearts	\N	GameSpy Hearts	f
403	gsbgammon	\N	GameSpy Backgammon	f
404	gscheckers	\N	GameSpy Checkers	f
405	atlantica	\N	Atlantica	f
406	merchant2	\N	Merchant Prince II	f
407	magmay2d	\N	The Art of War	f
408	assimilation	\N	Assimilation	f
409	zax	\N	Zax	f
410	leadfoot	\N	Leadfoot	f
411	leadfootd	\N	Leadfoot Demo	f
412	chat	\N	Chat Service	f
413	disciples	\N	Disciples	f
414	opflash	\N	Operation Flashpoint	f
415	zsteel	\N	Z: Steel Soldiers	f
416	redlinenet	\N	Redline Multi-Player Inst	f
417	gschess	\N	GameSpy Chess	f
418	gsreversi	\N	GameSpy Reversi	f
419	gsyarn	\N	GameSpy Y.A.R.N.	f
420	tribes2	\N	Tribes 2	f
421	avp2	\N	Aliens vs Predator 2	f
422	bodarkness	\N	Blade of Darkness	f
423	dominion	\N	Dominion	f
424	disciples2	\N	Disciples 2	f
425	opflashd	\N	Operation Flashpoint Demo	f
426	blade	\N	Blade	f
427	mechcomm	\N	MechCommander	f
428	globalops	\N	Global Operations	f
429	links99	\N	Links LS 1999	f
430	rulesotg	\N	Rules of the Game	f
431	avpnotgold	\N	Aliens vs. Predator	f
432	armymen	\N	Army Men	f
433	_news	\N	News	f
434	railsamd	\N	Rails Across America Demo	f
435	railsam	\N	Rails Across America	f
436	kohanexp	\N	Kohan Expansion	f
437	cueballworld	\N	Jimmy White Cueball World	f
438	wz2100demo	\N	Warzone 2100 (Demo)	f
439	sfc2opdv	\N	Starfleet Command II: Orion Pirates (Dynaverse II)	f
440	roguespeard	\N	Rogue Spear Demo	f
441	redalert	\N	Red Alert	f
442	wormsarm	\N	Worms Armageddon	f
443	takingdoms	\N	TA: Kingdoms	f
444	arc	\N	Arc: Sierra	f
445	diablo	\N	Diablo	f
446	tetrisworlds	\N	Tetris Worlds	f
447	dh5	\N	Deer Hunter 5	f
448	diablo2	\N	Diablo 2	f
449	starcraft	\N	Starcraft	f
450	starcraftdmo	\N	Starcraft Demo	f
451	rsblackthorn	\N	Rogue Spear: Black Thorn	f
452	warcraft2bne	\N	Warcraft 2	f
453	redalert2	\N	Red Alert 2	f
454	projecteden	\N	Project Eden	f
455	roadwars	\N	Road Wars	f
456	tiberiansun	\N	Tiberian Sun	f
457	chessworlds	\N	Chess Worlds	f
458	americax	\N	America Addon	f
459	sfc2op	\N	Starfleet Command: Orion	f
460	warlordsdr	\N	Warlords III: Dark Rising	f
461	stef1exp	\N	Star Trek: Voyager - Elite Force expansion pack	f
462	cmanager	\N	Cycling Manager	f
463	laserarenad	\N	Laser Arena Demo	f
464	starcraftexp	\N	Starcraft: Brood Wars	f
465	tsfirestorm	\N	Tiberian Sun - Firestorm	f
466	monopolyty	\N	Monopoly Tycoon	f
467	thps3ps2	\N	Tony Hawk Pro Skater 3 (PS2)	f
468	emperorbfd	\N	Emperor: Battle For Dune	f
469	claw	\N	Claw	f
470	armymenrts	\N	Army Men RTS	f
471	legendsmmbeta2	\N	Legends of Might and Magic First Look 2	f
472	conquestfw	\N	Conquest: Frontier Wars	f
473	realwar	\N	Real War	f
474	axis	\N	Axis	f
475	anno1503	\N	Anno 1503	f
476	incomingforces	\N	Incoming Forces	f
477	ironstrategy	\N	Iron Strategy	f
478	heavygear2	\N	Heavy Gear 2	f
479	sfc2dv	\N	Starfleet Command 2: Empires At War Dynaverse	f
480	motoracer3	\N	Motoracer 3	f
481	rogerwilco	\N	Roger Wilco	f
482	conquestfwd	\N	Conquest: Frontier Wars D	f
483	thps3media	\N	Tony Hawk Pro Skater 3 Media	f
484	echelon	\N	Echelon	f
485	takeda	\N	Takeda	f
486	cnoutbreak	\N	Codename: Outbreak	f
487	oldscrabble	\N	Scrabble 1.0	f
488	st_highscore	\N	Stats and Tracking Sample	f
489	rdpoker	\N	Reel Deal Poker	f
490	f1teamdriver	\N	Williams F1 Team: Team Dr	f
491	aquanox	\N	Aquanox	f
492	mohaa	\N	Medal of Honor Allied Assault	f
493	harley3	\N	Harel 3	f
494	cnoutbreakd	\N	Codename: Outbreak Demo	f
495	ras	\N	Red Ace Squadron	f
496	opfor	\N	Opposing Force	f
497	rallychampx	\N	Rally Championship Extrem	f
498	austerlitz	\N	Austerlitz: Napoleons Gre	f
499	dmania	\N	DMania	f
500	bgatetales	\N	Baldur's Gate: Tales of the Sword Coast	f
501	cueballworldd	\N	Cueball World Demo	f
502	st_rank	\N	Global Rankings Sample	f
503	rallytrophy	\N	Rally Trophy	f
504	kohanagdemo	\N	Kohan: Ahrimans Gift Demo	f
505	actval1	\N	Activision Value Title 1	f
506	etherlords	\N	Etherlords	f
507	swine	\N	S.W.I.N.E.	f
508	warriorkings	\N	Warrior Kings	f
509	myth3	\N	Myth 3	f
517	commandos2	\N	Commandos 2	f
518	mcomm2	\N	MechCommander 2	f
519	masterrally	\N	Master Rally	f
520	praetorians	\N	Praetorians	f
521	iwd2	\N	Icewind Dale 2	f
522	gamebot	\N	GameBot Test	f
523	armada2beta	\N	Star Trek: Armada 2 Beta	f
524	rtcwtest	\N	Wolfenstein MP Test	f
525	mechcomm2	\N	MechCommander 2	f
526	nvchess	\N	nvChess	f
527	msecurity	\N	Alcatraz: Prison Escape	f
528	avp2demo	\N	Aliens vs Predator 2 Demo	f
529	swgbd	\N	Star Wars: Galactic Battlegrounds Demo	f
530	chaser	\N	Chaser	f
531	nascar5	\N	NASCAR 5	f
532	kohanag	\N	Kohan: Ahrimans Gift	f
533	tribes2demo	\N	Tribes 2 Demo	f
534	serioussamse	\N	Serious Sam: Second Encounter	f
535	st_ladder	\N	Global Rankings Sample - Ladder	f
536	swgb	\N	Star Wars: Galactic Battlegrounds	f
537	bumperwars	\N	Bumper Wars!	f
538	combat	\N	Combat	f
539	etherlordsd	\N	Etherlords Demo	f
540	rsblackthornd	\N	Black Thorn Demo	f
541	bfield1942	\N	Battlefield 1942	f
542	battlerealmsbBAD	\N	Battle Realms Beta	f
543	swinedemo	\N	Swine Demo	f
544	freedomforce	\N	Freedom Force	f
545	il2sturmovik	\N	IL-2 Sturmovik	f
548	myth3demo	\N	Myth 3 Demo	f
549	strongholdd	\N	Stronghold Demo	f
550	ghostrecon	\N	Tom Clancy's Ghost Recon	f
551	ghostrecond	\N	Tom Clancy's Ghost Recon Demo	f
552	fltsim2002	\N	Microsoft Flight Simulator 2002	f
553	mech4bkexp	\N	MechWarrior Black Knight	f
554	hd	\N	Hidden & Dangerous Enhanc	f
555	strifeshadowd	\N	Strifeshadow Demo	f
556	conflictzone	\N	Conflict Zone	f
557	racedriver	\N	TOCA Race Driver	f
558	druidking	\N	Druid King	f
559	itycoon2	\N	Industry Tycoon 2	f
560	sof2	\N	Soldier of Fortune 2	f
561	armada2d	\N	Star Trek: Armada II Demo	f
562	avp2lv	\N	Aliens vs. Predator 2 (Low violence)	f
563	rtcw	\N	Return to Castle Wolfenstein	f
564	xboxtunnel	\N	Xbox Tunnel Service	f
565	survivor	\N	Survivor Ultimate	f
566	il2sturmovikd	\N	IL-2 Sturmovik Demo	f
567	haegemonia	\N	Haegemonia	f
568	mohaad	\N	Medal of Honor: Allied Assault Demo	f
569	serioussamsed	\N	Serious Sam: Second Encounter Demo	f
570	janesf18	\N	Janes F/A-18	f
571	janesusaf	\N	Janes USAF	f
572	janesfa	\N	Janes Fighters Anthology	f
573	janesf15	\N	Janes F-15	f
574	janesww2	\N	Janes WWII Fighters	f
575	mech4bwexpd	\N	MechWarrior Black Knight	f
576	f12002	\N	F1 2002	f
577	ccrenegade	\N	Command & Conquer: Renegade	f
578	redalert2exp	\N	Command & Conquer: Yuri's Revenge	f
579	capitalism2	\N	Capitalism 2	f
580	demoderby	\N	Demolition Derby & Figure	f
581	janesattack	\N	Janes Attack Squadron	f
582	chesk	\N	Chesk	f
583	hhball2003	\N	High Heat Baseball 2003	f
584	duelfield	\N	Duelfield	f
585	carnivores3	\N	Carnivores 3	f
586	jk2	\N	Star Wars Jedi Knight II: Jedi Outcast	f
587	thps3pc	\N	Tony Hawk 3 (PC)	f
588	blockade	\N	Operation Blockade	f
589	mafia	\N	Mafia	f
590	ccrenegadedemo	\N	Command & Conquer: Renegade Demo	f
591	shadowforce	\N	Shadow Force: Razor Unit	f
595	gta3pc	\N	Grand Theft Auto 3 (PC)	f
596	vietkong	\N	Vietkong	f
597	mooncommander	\N	Moon Commander	f
598	subcommand	\N	Sub Command	f
599	originalwar	\N	Original War	f
600	thps4ps2	\N	Tony Hawk: Pro Skater 4 (PS2)	f
601	warlordsb2d	\N	Warlords Battlecry II Demo	f
602	ioftheenemy	\N	I of the Enemy	f
603	sharpshooter	\N	Sharp Shooter	f
604	medieval	\N	Medieval: Total War	f
605	globalopspb	\N	Global Operations Public Beta	f
606	pb4	\N	Extreme Paintbrawl 4	f
610	armygame	\N	Americas Army: Special Forces	f
611	homm4	\N	Heroes of Might and Magic	f
612	darkplanet	\N	Dark Planet	f
613	mobileforces	\N	Mobile Forces	f
614	teamfactor	\N	Team Factor	f
615	dragonthrone	\N	Dragon Throne	f
616	celebdm	\N	Celebrity Deathmatch	f
617	phoenix	\N	Phoenix (Stainless Steel)	f
618	matrixproxy	\N	Matrix Proxy	f
619	mobileforcesd	\N	Mobile Forces Demo	f
620	ghostreconds	\N	Ghost Recon: Desert Siege	f
621	sof2demo	\N	Soldier of Fortune 2 Demo	f
622	etherlordsbeta	\N	Etherlords Patch Beta	f
623	aow2d	\N	Age of Wonders 2 Demo	f
624	privateer	\N	Privateers Bounty: Age of Sail 2	f
625	gcracing	\N	Great Clips Racing	f
626	survivorm	\N	Survivor: Marquesas	f
627	dungeonsiege	\N	Dungeon Siege	f
628	silenthunter2	\N	Silent Hunter 2	f
629	celtickings	\N	Druid King	f
630	globalopsd	\N	Global Ops Demo	f
631	renegadebf	\N	Renegade Battlefield	f
632	warlordsb2	\N	Warlords Battlecry II	f
633	tacticalops	\N	Tactical Ops	f
634	ut2	\N	Unreal Tournament 2003	f
635	swgbcc	\N	Star Wars Galactic Battle	f
636	ut2d	\N	Unreal Tournament 2003 Demo	f
637	voiceapp	\N	VoiceApp Voice SDK Test	f
638	sumofallfearsd	\N	The Sum of All Fears Demo	f
639	streetracer	\N	Streetracer	f
640	opflashr	\N	Operation Flashpoint: Resistance	f
641	mohaas	\N	Medal of Honor: Allied Assault Spearhead	f
642	avp2ph	\N	Aliens vs. Predator 2: Primal Hunt	f
643	nthunder2003	\N	NASCAR Thunder 2003	f
645	dtr2	\N	Dirt Track Racing II	f
646	GameSpy.com	\N	GameSpy.com	f
647	fileplanet	\N	FilePlanet.com	f
648	gored	\N	Gore Retail Demo	f
649	darkheaven	\N	Dark Heaven	f
650	twc	\N	Takeout Weight Curling	f
651	steeltide	\N	Operation Steel Tide	f
652	realwarrs	\N	Real War: Rogue States	f
653	ww2frontline	\N	World War II: Frontline Command	f
654	rmth2003	\N	Trophy Hunter 2003	f
655	strongholdc	\N	Stronghold: Crusader	f
656	soa	\N	Soldiers of Anarchy	f
657	jbnightfire	\N	James Bond: Nightfire	f
658	sumofallfears	\N	The Sum of All Fears	f
659	nfs6	\N	Need For Speed: Hot Pursuit 2	f
660	bangler2003	\N	Bass Angler 2003	f
661	netathlon2	\N	NetAthlon	f
662	sfc3	\N	Starfleet Command III	f
663	ddozenpt	\N	Deadly Dozen: Pacific Theater	f
664	vietnamso	\N	Line of Sight: Vietnam	f
665	mt2003	\N	Monopoly 2003	f
666	soad	\N	Soldiers of Anarchy Demo	f
667	celtickingsdemo	\N	Celtic Kings Demo	f
668	ironstorm	\N	Iron Storm	f
669	civ3ptw	\N	Civilization III: Play the World	f
670	tron20	\N	TRON 2.0	f
671	bfield1942d	\N	Battlefield 1942 Demo	f
672	scrabble3	\N	Scrabble 3	f
673	vietcong	\N	Vietcong	f
674	th2003d	\N	Trophy Hunter 2003 Demo	f
675	ccgenerals	\N	Command & Conquer: Generals	f
676	sfc3dv	\N	Starfleet Command III (Dynaverse)	f
677	bandits	\N	Bandits: Phoenix Rising	f
678	xar	\N	Xtreme Air Racing	f
679	echelonww	\N	Echelon Wind Warriors	f
683	dtr2d	\N	Dirt Track Racing 2 Demo	f
684	mclub2ps2	\N	Midnight Club 2 (PS2)	f
689	dh2003	\N	Deerhunter 2003	f
690	hwbasharena	\N	Hot Wheels Bash Arena	f
691	robotarena2	\N	Robot Arena 2	f
692	monopoly3	\N	Monopoly 3	f
693	banditsd	\N	Bandits: Phoenix Rising Demo	f
694	painkiller	\N	Painkiller	f
695	revolution	\N	Revolution	f
696	ddozenptd	\N	Deadly Dozen Pacific Theater Demo	f
697	ironstormd	\N	Iron Storm Demo	f
698	strikefighters1	\N	Strike Fighters: Project	f
699	moo3	\N	Master of Orion III	f
700	suddenstrike2	\N	Sudden Strike II	f
701	mostwanted	\N	Most Wanted	f
702	gicombat1	\N	G.I. Combat	f
703	projectigi2	\N	IGI 2: Covert Strike Demo	f
704	realwarrsd	\N	Real War: Rogue States Demo	f
705	pnomads	\N	Project Nomads	f
706	thps5ps2	\N	Tony Hawk's Underground (PS2)	f
707	strongholdcd	\N	Stronghold: Crusader Demo	f
708	blitzkrieg	\N	Blitzkrieg	f
709	woosc	\N	World of Outlaws Sprint Cars	f
710	vietcongd	\N	Vietcong Demo	f
711	hlwarriors	\N	Highland Warriors	f
712	mohaasd	\N	Medal of Honor: Allied As	f
713	bfield1942rtr	\N	Battlefield 1942: Road to Rome	f
714	horserace	\N	HorseRace	f
715	netathlon	\N	NetAthlon	f
716	ccgeneralsb	\N	Command & Conquer: Generals Beta	f
717	sandbags	\N	Sandbags and Bunkers	f
718	crttestdead	\N	CRT - TEST	f
719	nolf2	\N	No One Lives Forever 2	f
720	wkingsb	\N	Warrior Kings Battles	f
721	riseofnations	\N	Rise of Nations	f
722	worms3	fZDYBO	Worms 3D	f
723	castles	\N	Castles and Catapluts	f
724	mech4merc	\N	MechWarrior 4: Mercenarie	f
725	orbb	\N	O.R.B: Off-World Resource Base Beta	f
726	echelonwwd	\N	Echelon Wind Warriors Demo	f
728	snooker2003	\N	Snooker 2003	f
729	jeopardyps2	\N	Jeopardy (PS2)	f
730	riskps2	\N	Risk (PS2)	f
731	wofps2	\N	Wheel of Fortune (PS2)	f
732	dhunterps2	\N	Deer Hunter (PS2)	f
733	trivialppc	\N	Trivial Pursuit (PC) US	f
734	trivialpps2	\N	Trivial Pursuit (PS2)	f
735	projectigi2d	\N	IGI 2: Covert Strike Demo	f
736	projectigi2r	\N	IGI 2 Covert Strike	f
738	il2sturmovikfb	\N	IL-2 Sturmovik Forgotten Battles	f
739	wooscd	\N	World of Outlaws Sprint Cars Demo	f
740	nthunder2004	\N	NASCAR Thunder 2004	f
741	f1comp	\N	F1 1999-2000 Compilation	f
742	nomansland	\N	No Mans Land	f
743	nwnxp1	\N	Neverwinter Nights: Shado	f
744	praetoriansd	\N	Praetorians Demo	f
745	nrs2003	\N	NASCAR Racing Season 2003	f
746	gmtestam	\N	test (Auto-Matchmaking)	f
747	nolf2d	\N	No One Lives Forever: The Operative 2 Demo	f
748	devastation	\N	Devastation	f
749	blitz2004ps2	\N	NFL Blitz 2004 (PS2)	f
750	hd2	\N	Hidden and Dangerous 2	f
751	hd2b	\N	Hidden and Dangerous 2 Beta	f
754	hd2d	\N	Hidden and Dangerous 2 Demo	f
755	mrpantsqm	\N	Mr. Pants QuickMatch	f
756	moutlawne	\N	Midnight Outlaw Nitro	f
757	vietnamsod	\N	Line of Sight: Vietnam Demo	f
758	lionheart	\N	Lionheart	f
759	medievalvi	\N	Medieval Total War Viking Invasion	f
760	black9pc	\N	Black9 (PC)	f
761	black9ps2	\N	Black9 (PS2)	f
762	cmanager3	\N	Cycling Manager 3	f
764	devastationd	\N	Devastation Demo	f
765	hitz2004ps2	\N	NHL Hitz 2004 PS2	f
766	wkingsbd	\N	Warrior Kings Battles Demo	f
767	chaserd	\N	Chaser Demo	f
768	motogp2	\N	MotoGP 2	f
769	motogp2d	\N	MotoGP 2 Demo	f
770	racedriverd	\N	Race Driver Demo	f
771	empiresam	\N	Empires Dawn of the Modern World (AM)	f
772	empires	\N	Empires: Dawn of the Modern World	f
773	crashnitro	\N	Crash Nitro Carts	f
774	breed	\N	Breed	f
775	breedd	\N	Breed Demo	f
776	homeworld2	\N	Homeworld 2	f
777	moo3a	\N	Master of Orion III	f
778	nwnmac	\N	Neverwinter Nights (Mac)	f
779	ravenshield	\N	Raven Shield	f
780	stef2	\N	Star Trek: Elite Force II	f
781	spacepod	\N	SpacePod	f
782	agrome	\N	Against Rome	f
783	bfield1942sw	\N	Battlefield 1942: Secret Weapons of WW2	f
784	thps4pc	\N	Tony Hawk: Pro Skater 4 (PC)	f
785	omfbattled	\N	One Must Fall Battlergounds Demo	f
786	nwnlinux	\N	Neverwinter Nights (Linux)	f
787	blitz2004ps2e	\N	NFL Blitz Pro 2004 E3 (PS2)	f
788	blitz2004ps2b	\N	NFL Blitz Pro 2004 Beta (PS2)	f
789	homeworld2b	\N	Homeworld 2 Beta	f
790	halo	\N	Halo Beta	f
791	lotr3	\N	Lords of the Realm III	f
792	lotr3b	\N	Lords of the Realm III Beta	f
793	halor	\N	Halo: Combat Evolved	f
794	bllrs2004ps2	\N	NBA Ballers (PS2)	f
795	rtcwett	\N	Wolfenstein: Enemy Territory Test	f
796	mclub2pc	\N	Midnight Club 2 (PC)	f
797	jacknick6	\N	Jack Nicklaus Golden Bear	f
798	wotr	\N	War of the Ring	f
799	terminator3	\N	Terminator 3	f
800	fwarriorpc	\N	Fire Warrior	f
801	fwarriorps2	\N	Fire Warrior (PS2)	f
802	mohaab	\N	Medal of Honor: Allied Assault Breakthrough	f
803	aow3	\N	Age of Wonders: Shadow Magic (aow3)	f
804	E3_2003	\N	E3_2003	f
805	aowsm	\N	Age of Wonders: Shadow Magic (aowsm)	f
806	specialforces	\N	Special Forces	f
807	spartan	\N	Spartan & Spartan	f
808	dod	\N	Day of Defeat	f
809	tron20d	\N	TRON 2.0 Demo	f
810	omfbattleb	\N	One Must Fall Battlegrounds	f
811	bfield1942swd	\N	Battlefield 1942: Secret Weapons of WW2 Demo	f
813	rtcwet	\N	Wolfenstein: Enemy Territory	f
814	mphearts	\N	mphearts	f
815	hotrod	\N	Hot Rod, American Street Drag	f
816	civ3con	\N	Civilization III: Conquests	f
817	civ3conb	\N	Civilization III: Conquests Beta	f
818	riseofnationsam	\N	Rise of Nations Auto-Matching	f
819	afrikakorps	\N	Afrika Korps	f
820	apocalypticadi	\N	Apocalyptica	f
821	robotech2	\N	Robotech 2 (PS2)	f
822	spacepodd	\N	Space Pod Demo	f
823	ccgenzh	\N	Command & Conquer: Generals – Zero Hour	f
824	ronb	\N	Rise of Nations Beta	f
825	ronbam	\N	Rise of Nations Beta (Automatch)	f
826	commandos3	\N	Commandos 3	f
827	exigo	\N	Armies of Exigo	f
828	dh2004	\N	Deer Hunter 2004	f
830	dh2004d	\N	Deer Hunter 2004 Demo	f
831	armygamemac	\N	Americas Army: Special Forces (Mac)	f
832	bridgebaron14	\N	Bridge Baron	f
833	jk3	\N	Star Wars Jedi Knight: Jedi Academy	f
834	anno1503b	\N	Anno 1503 Beta	f
835	contractjack	\N	Contract Jack	f
836	postal2	\N	Postal 2	f
837	ut2004	\N	Unreal Tournament 2004	f
838	ut2004d	\N	Unreal Tournament 2004 Demo	f
839	contractjackd	\N	Contract Jack Demo	f
840	empiresd	\N	Empires: Dawn of the Modern World Demo	f
1104	regimentps2	\N	The Regiment PS2	f
841	empiresdam	\N	Empires: Dawn of the Modern World	f
842	mtgbgrounds	\N	Magic The Gathering: Battlegrounds	f
843	groundcontrol2	\N	Ground Control 2	f
844	bfield1942ps2	\N	Battlefield Modern Combat (PS2)	f
845	dsiege2	\N	Dungeon Siege 2 The Azunite Prophecies	f
846	judgedredddi	\N	Judge Dredd	f
847	coldwinter	\N	Cold Winter	f
848	haegemoniaxp	\N	Hegemonia Expansion	f
849	asbball2005ps2	\N	All-star Baseball 2005	f
850	castlestrike	\N	Castle Strike	f
851	homeworld2d	\N	Homeworld 2 Demo	f
852	callofduty	\N	Call of Duty	f
853	mohaabd	\N	Medal of Honor: Allied Assault Breakthrough Demo	f
854	twc2	\N	Takeout Weight Curling 2	f
855	nthunder2004d	\N	NASCAR Thunder 2004 Demo	f
856	moutlawned	\N	Midnight Outlaw Illegal Street Drag Nitro Edition Demo	f
857	mta	\N	Multi Theft Auto	f
859	railty3	\N	Railroad Tycoon 3	f
860	spellforce	\N	Spellforce	f
861	halomac	\N	Halo (Mac)	f
862	contractjackpr	\N	Contract Jack PR	f
864	wotrb	\N	War of the Ring Beta	f
865	halod	\N	Halo Demo	f
866	wcpool2004ps2	\N	World Championship Pool 2004 (PS2)	f
867	fairstrike	\N	Fair Strike	f
868	aarts	\N	Axis and Allies RTS	f
1222	motogp3d	\N	MotoGP 3 Demo	f
869	nwnxp2	\N	Neverwinter Nights: Hordes of Underdark	f
870	lotrbme	\N	Lord of the Rings: The Battle For Middle-Earth	f
871	mototrax	\N	Moto Trax	f
872	painkillerd	\N	Painkiller Demo	f
873	painkillert	\N	Painkiller Multiplayer Test	f
874	entente	\N	The Entente	f
876	sforces	\N	Special Forces	f
877	slugfestps2	\N	Slugfest Pro (PS2)	f
878	mohpa	\N	Medal of Honor: Pacific Assault	f
879	battlemages	\N	Battle Mages	f
880	bfvietnam	\N	Battlefield: Vietnam	f
881	planetside	\N	PlanetSide	f
882	daoc	\N	Dark Age of Camelot	f
883	uotd	\N	Ultima Online Third Dawn	f
884	swg	\N	Star Wars Galaxies	f
885	eq	\N	Everquest	f
886	kohankow	\N	Kohan: Kings of War	f
887	serioussamps2	\N	Serious Sam (PS2)	f
888	omfbattlecp	\N	One Must Fall Battlegrounds (GMX)	f
889	fairstriked	\N	Fair Strike Demo	f
890	celtickingspu	\N	Nemesis of the Roman Empire	f
891	test	\N	Test	f
892	truecrime	\N	True Crime	f
895	unreal2d	\N	Unreal 2 Demo	f
896	links2004	\N	Links 2004	f
897	terminator3d	\N	Terminator 3 Demo	f
900	wcpool2004pc	\N	World Championship Pool 2004	f
901	postal2d	\N	Postal 2 Demo	f
902	unreal2demo	\N	Unreal 2 Demo	f
903	spellforced	\N	Spellforce Demo	f
904	le_projectx	\N	Legend Entertainment Project X	f
905	racedriver2	\N	Race Driver 2	f
906	bomberfunt	\N	BomberFUN Tournament	f
907	pbfqm	\N	PlanetBattlefield QuickMatch	f
908	gangland	\N	Gangland	f
909	halomacd	\N	Halo Demo (Mac)	f
910	juicedpc	\N	Juiced (PC)	f
911	juicedps2	\N	Juiced (PS2)	f
913	tribesv	\N	Tribes Vengeance	f
914	racedriver2ps2	\N	Race Driver 2 (PS2)	f
915	racedriver2d	\N	Race Driver 2 Demo	f
916	indycarps2	\N	Indycar Series (PS2)	f
917	thps6ps2	\N	Tony Hawks Underground 2 (PS2)	f
918	sniperelps2	\N	Sniper Elite (PS2)	f
920	bllrs2004ps2d	\N	NBA Ballers Demo (PS2)	f
921	saturdayns	\N	Saturday Night Speedway	f
922	rometw	\N	Rome: Total War	f
923	conan	\N	Conan: The Dark Axe	f
924	rontp	\N	Rise of Nations: Throne and Patriots	f
925	rontpam	\N	Rise of Nations: Throne and Patriots (Automatch)	f
926	dmhand	\N	Dead Man Hand	f
927	upwords	\N	upwords	f
928	saturdaynsd	\N	Saturday Night Speedway Demo	f
929	scrabbledel	\N	Scrabble Deluxe	f
930	dsiege2am	\N	Dungeon Siege 2 The Azunite Prophecies (Automatch)	f
931	cmr4pc	\N	Colin McRae Rally 4 (PC)	f
932	kumawar	\N	Kuma War	f
933	cmr4pcd	\N	Colin McRae Rally 4 Demo (PC)	f
939	afrikakorpsd	\N	Desert Rats vs. Afrika Korps Demo	f
940	crashnburnps2	\N	Crash N Burn (PS2)	f
941	spartand	\N	Spartan Demo	f
942	ace	\N	A.C.E.	f
944	perimeter	\N	Perimeter	f
945	ilrosso	\N	Il Rosso e Il Nero - The Italian Civil War	f
946	whammer40000	\N	Warhammer 40,000: Dawn of War	f
947	swat4	\N	SWAT 4	f
948	eearth2	h3C2jU	Empire Earth 2	f
949	tribesvd	\N	Tribes Vengeance Demo	f
950	tribesvb	\N	Tribes Vengeance Beta	f
951	ganglandd	\N	Gangland Demo	f
952	sniperelpc	\N	Sniper Elite (PC)	f
954	altitude	\N	Altitude	f
955	fsx	\N	Flight Simulator 2006	f
956	hotwheels2pc	\N	Hot Wheels 2 (PC)	f
957	hotwheels2ps2	\N	Hot Wheels 2 (PS2)	f
958	hotwheels2pcd	\N	Hot Wheels 2 Demo (PC)	f
959	cnpanzers	\N	Codename Panzers	f
960	gamepopulator	\N	Game Populator	f
961	gamepopulatoram	\N	Game Populator (Automatch)	f
963	livewire	\N	GameSpy Livewire	f
964	ravenshieldas	\N	Raven Shield: Athena's Sword	f
965	fear	\N	FEAR: First Encounter Assault Recon	f
966	tron20mac	\N	TRON 2.0 (Mac)	f
967	s_cstrikecz	\N	Steam Counter-Strike: Condition Zero	f
968	wingsofwar	\N	Wings of War	f
969	mxun05ps2	\N	MX Unleashed 05 (PS2)	f
970	mxun05ps2am	\N	MX Unleashed 05 (PS2) (Automatch)	f
971	swbfrontps2	\N	Star Wars: Battlefront (PS2, Japan)	f
973	swbfrontpc	\N	Star Wars: Battlefront (PC)	f
974	perimeterd	\N	Perimeter Demo	f
975	wracing1	\N	World Racing 1	f
976	wormsforts	y3Gc8n	Worms Forts: Under Siege	f
977	mohaamac	\N	Medal of Honor: Allied Assault (Mac)	f
978	mohaasmac	\N	Medal of Honor: Allied Assault Spearhead (Mac)	f
979	mohaabmac	\N	Medal of Honor: Breakthrough (Mac)	f
980	bfield1942mac	\N	Battlefield 1942 (Mac)	f
981	bfield1942rtrm	\N	Battlefield 1942 Road to Rome (Mac)	f
982	halom	\N	Halo Multiplayer Expansion	f
983	nitrofamily	\N	Nitro Family	f
984	besieger	\N	Besieger	f
986	mkdeceptionps2	\N	Mortal Kombat Deceptions (PS2)	f
987	swrcommando	\N	Star Wars: Republic Commando	f
988	fightclubps2	\N	Fight Club (PS2)	f
989	area51ps2	\N	Area 51 (PS2)	f
990	dday	\N	D-Day	f
991	exigoam	\N	Armies of Exigo (Automatch)	f
992	mohaabdm	\N	Medal of Honor: Allied Assault Breakthrough Demo (Mac)	f
993	mkdeceppalps2	\N	Mortal Kombat Deception PAL (PS2)	f
994	civ4b	\N	Civilization 4 Beta	f
995	topspin	\N	Top Spin	f
996	bllrs2004pal	\N	NBA Ballers PAL (PS2)	f
997	whammer40000am	\N	Warhammer 40,000: Dawn of War	f
999	scrabbleo	\N	Scrabble Online	f
1000	wcsnkr2004ps2	\N	World Championship Snooker 2004 (PS2)	f
1001	olg2PS2	\N	Outlaw Golf 2 PS2	f
1002	gtasaps2	\N	Grand Theft Auto San Andreas (PS2)	f
1003	thps6pc	\N	T.H.U.G. 2	f
1004	smackdnps2	\N	WWE Smackdown vs RAW Sony Beta (PS2)	f
1005	thps5pc	\N	Tony Hawks Underground (PC)	f
1006	menofvalor	\N	Men of Valor	f
1007	srsyndps2	\N	Street Racing Syndicate (PS2)	f
1008	gc2demo	\N	Ground Control 2 Demo	f
1009	fswpc	R5pZ29	Full Spectrum Warrior	f
1010	soldiersww2	\N	Soldiers: Heroes of World War II	f
1011	mtxmototrax	\N	MTX MotoTrax	f
1012	pbfqmv	\N	PlanetBattlefield QuickMatch Vietnam	f
1013	wcsnkr2004pc	\N	World Championship Snooker 2004 (PC)	f
1014	locomotion	\N	Chris Sawyer's Locomotion	f
1015	gauntletps2	\N	Gauntlet (PS2)	f
1016	gotcha	\N	Gotcha!	f
1017	menofvalord	\N	Men of Valor Demo	f
1019	knightsoh	\N	Knights of Honor	f
1020	wingsofward	\N	Wings of War Demo	f
1021	cmr5ps2	\N	Colin McRae Rally 5 (PS2)	f
1022	callofdutyps2	\N	Call of Duty (PS2)	f
1023	crashnburnps2b	\N	Crash N Burn Sony Beta (PS2)	f
1024	hotrod2	\N	Hot Rod 2: Garage to Glory	f
1025	mclub3ps2	\N	Midnight Club 3 DUB Edition (PS2)	f
1027	trivialppalps2	\N	Trivial Pursuit PAL (PS2)	f
1028	trivialppalpc	\N	Trivial Pursuit PAL (PC)	f
1029	hd2ss	\N	Hidden & Dangerous 2 - Sabre Squadron	f
1030	whammer40kb	\N	Warhammer 40,000: Dawn of War Beta	f
1031	whammer40kbam	\N	Warhammer 40,000: Dawn of War Beta (Automatch)	f
1032	srsyndpc	\N	Street Racing Syndicate (PC)	f
1033	ddayd	\N	D-Day Demo	f
1034	godzilla2ps2	\N	Godzilla: Save the Earth (PS2)	f
1035	actofwar	\N	Act of War: Direct Action	f
1036	juicedpalps2	\N	Juiced PAL (PS2)	f
1037	statesmen	\N	Statesmen	f
1038	conflictsopc	vh398A	Conflict: Special Ops (PC)	f
1039	conflictsops2	\N	Conflict: Special Ops (PS2)	f
1040	dh2005	\N	Deer Hunter 2005	f
1041	gotchad	\N	Gotcha! Demo	f
1042	eearth2d	h3C2jU	Empire Earth 2 Demo	f
1043	smackdnps2pal	\N	WWE Smackdown vs RAW PAL (PS2)	f
1044	wcpokerps2	\N	World Championship Poker (PS2)	f
1045	cmr5pc	\N	Colin McRae Rally 5 (PC)	f
1046	dh2005d	\N	Deer Hunter 2005 Demo	f
1048	callofdutyps2d	\N	Call of Duty Sony Beta (PS2)	f
1049	doom3	\N	Doom 3	f
1050	cmr5pcd	\N	Colin McRae Rally 5 Demo (PC)	f
1051	spoilsofwar	\N	Spoils of War	f
1052	saadtest	\N	SaadsTest	f
1054	superpower2	\N	Super Power 2	f
1055	swat4d	\N	SWAT 4 Demo	f
1056	exigob	\N	Armies of Exigo Beta	f
1057	exigobam	\N	Armies of Exigo Beta (Automatch)	f
1058	knightsohd	\N	Knights of Honor Demo	f
1059	battlefield2	\N	Battlefield 2	f
1060	actofwaram	\N	Act of War: Direct Action (Automatch)	f
1061	bf1942swmac	\N	Battlefield 1942: Secret Weapons of WW2 Mac	f
1062	closecomftf	\N	Close Combat: First to Fight	f
1063	closecomftfmac	\N	Close Combat: First to Fight Mac	f
1064	kohankowd	\N	Kohan: Kings of War Demo	f
1066	swempire	\N	Star Wars: Empire at War	f
1067	stalkersc	\N	STALKER: Shadows of Chernobyl	f
1068	poolshark2ps2	\N	Pool Shark 2 (PS2)	f
1069	poolshark2pc	\N	Pool Shark 2 (PC)	f
1070	smackdnps2kor	\N	WWE Smackdown vs RAW (PS2) Korean	f
1071	smackdnps2r	\N	WWE Smackdown vs RAW (PS2) Retail	f
1072	callofdutyuo	\N	Call of Duty: United Offensive	f
1073	swbfrontps2p	\N	Star Wars: Battlefront (PS2)	f
1074	trivialppcuk	\N	Trivial Pursuit (PC) UK	f
1075	trivialppcfr	\N	Trivial Pursuit (PC) French	f
1076	trivialppcgr	\N	Trivial Pursuit (PC) German	f
1077	trivialppcit	\N	Trivial Pursuit (PC) Italian	f
1078	trivialppcsp	\N	Trivial Pursuit (PC) Spanish	f
1079	smackdnps2palr	\N	WWE Smackdown vs RAW (PS2) PAL Retail	f
1080	aartsd	\N	Axis and Allies RTS demo	f
1081	blitzkriegrt	\N	Blitzkrieg: Rolling Thunder	f
1082	dungeonlords	\N	Dungeon Lords	f
1083	SpyNote	\N	Server Monitor	f
1085	blitz2005ps2	\N	Blitz: The League 2005	f
1086	rof	\N	Rise of Legends	f
1087	rofam	\N	Rise of Legends (Automatch)	f
1088	nsr0405	\N	NASCAR Sim Racing (2005)	f
1089	ffvsttr	\N	Freedom Force vs. The Third Reich	f
1092	dshard	\N	The Dragonshard Wars	f
1093	mohpad	\N	Medal of Honor: Pacific Assault Demo	f
1094	exigor	\N	Armies of Exigo Retail	f
1095	exigoram	\N	Armies of Exigo (Automatch)	f
1096	bfield1942t	\N	Battlefield 1942 Testing	f
1099	bfvietnamt	\N	Battlefield: Vietnam Testing	f
1101	civ4	y3D9Hw	Civilization IV	f
1102	civ4am	y3D9Hw	Civilization IV (Automatch)	f
1103	regimentpc	\N	The Regiment PC	f
1105	olvps2	\N	Outlaw Volleyball PS2	f
1106	battlefield2d	\N	Battlefield 2 Demo	f
1108	fswps2	\N	Full Spectrum Warrior PS2	f
1109	dshardam	\N	The Dragonshard Wars (Automatch)	f
1110	spoilsofwaram	\N	Spoils of War (Automatch)	f
1111	source	\N	Half Life 2	f
1112	s_cssource	\N	Counter-Strike Source	f
1113	feard	\N	FEAR: First Encounter Assault Recon Demo	f
1114	s_hl2dm	\N	s_hl2dm	f
1115	bfield1942ps2b	\N	Battlefield Modern Combat (PS2) Beta	f
1116	whammer40kt	\N	Warhammer 40000: Dawn of War test	f
1117	firecapbay	\N	Fire Captain: Bay Area Inferno	f
1118	splintcellchaos	\N	splintcellchaos	f
1119	fswps2pal	\N	Full Spectrum Warrior PAL PS2	f
1120	fearcb	\N	FEAR: First Encounter Assault Recon (Closed Beta)	f
1121	fearob	\N	FEAR: First Encounter Assault Recon (Open Beta)	f
1122	ejammingpc	\N	eJamming Jamming Station PC	f
1123	ejammingmac	\N	eJamming Jamming Station MAC (engine)	f
1124	wcpokerpalps2	\N	World Championship Poker PAL (PS2)	f
1125	titanquest	\N	Titan Quest	f
1126	wcsnkr2005ps2	\N	World Championship Snooker 2005 PS2	f
1127	wcsnkr2005	\N	World Championship Snooker 2005 (PC)	f
1128	thps7ps2	\N	Tony Hawks American Wasteland (PS2)	f
1129	pariahpc	\N	Pariah (PC)	f
1130	impglory	\N	Imperial Glory	f
1132	swrcommandoj	\N	Star Wars Republic Commando Japanese Dist	f
1133	oltps2	\N	Outlaw Tennis PS2	f
1134	wptps2	\N	World Poker Tour PS2	f
1135	blkhwkdnps2	\N	Delta Force: Black Hawk Down (PS2)	f
1137	motogp3	\N	MotoGP 3	f
1138	cmmwcpoker	\N	Chris Moneymakers World Championship Poker	f
1139	ddayxp1	\N	D-Day: 1944 Battle of the Bulge	f
1140	spcell3coop	\N	Splinter Cell 3 CoOp	f
1142	ffvsttrd	\N	Freedom Force vs. The Third Reich MP Demo	f
1143	topspinps2	\N	Top Spin (PS2)	f
1144	betonsoldier	\N	Bet on Soldier	f
1145	swrcommandot	\N	Star Wars Republic Commando Thai Dist	f
1146	topspinps2am	\N	Top Spin (PS2) (Automatch)	f
1147	vietcong2	\N	Vietcong 2	f
1148	spyvsspyps2	\N	Spy vs Spy (PS2)	f
1149	nitrosample	\N	Nitro Sample	f
1150	flatoutps2	\N	Flat Out (PS2)	f
1151	hotpacificps2	\N	Heroes of the Pacific (PS2)	f
1152	hotpacificpc	\N	Heroes of the Pacific (PC)	f
1154	cnpanzers2	\N	Codename Panzers Phase 2	f
1155	stronghold2	\N	Stronghold 2	f
1156	actofward	\N	Act of War: Direct Action Demo	f
1157	actofwardam	\N	Act of War: Direct Action Demo (Automatch)	f
1158	xmenlegpc	\N	X-Men Legends (PC)	f
1159	xmenlegps2	\N	X-Men Legends (PS2)	f
1160	coteagles	\N	War Front: Turning Point	f
1161	area51pc	\N	Area 51 (PC)	f
1164	area51pcb	\N	Area 51 (PC) Beta	f
1165	fswps2kor	\N	Full Spectrum Warrior Korean (PS2)	f
1169	stalinsubd	\N	The Stalin Subway Demo	f
1170	supruler2010	\N	Supreme Ruler 2010	f
1171	pariahpcd	\N	Pariah Demo (PC)	f
1172	serioussam2	\N	Serious Sam 2 (PC)	f
1173	riskingdoms	\N	Rising Kingdoms	f
1176	stalinsub	\N	The Stalin Subway	f
1177	bsmidwaypc	\N	Battlestations Midway (PC)	f
1178	bsmidwayps2	\N	Battlestations Midway (PS2)	f
1179	bsmidwaypcam	\N	Battlestations Midway (PC) (Automatch)	f
1180	bsmidwayps2am	\N	Battlestations Midway PS2 (Automatch)	f
1181	riskingdomsd	\N	Rising KIngdoms Demo	f
1182	riskingdomsam	\N	Rising Kingdoms (Automatch)	f
1183	wsoppc	\N	World Series of Poker (PC)	f
1184	wsopps2	\N	World Series of Poker (PS2)	f
1185	velocityps2	\N	Velocity PS2	f
1186	velocitypc	\N	Velocity PC	f
1187	swat4xp1	\N	SWAT 4: The Stetchkov Syndicate	f
1188	hotpaceudps2	\N	Heroes of the Pacific EU Demo (PS2)	f
1189	hotpacnadps2	\N	Heroes of the Pacific NA Demo (PS2)	f
1190	gbrome	hEf6s9j	Great Battles of Rome	f
1191	rafcivatwar	\N	Rise And Fall: Civilizations at War	f
1193	rafcivatwaram	\N	Rise And Fall: Civilizations at War (Automatch)	f
1194	fearobsc	\N	FEAR: First Encounter Assault Recon (Open Beta Special Content)	f
1195	worms4	Bs28Kl	Worms 4 Mayhem	f
1196	smackdn2ps2	\N	WWE Smackdown vs RAW 2 (PS2)	f
1197	smackdn2ps2pal	\N	WWE Smackdown vs RAW 2 PAL (PS2)	f
1198	smackdn2ps2kor	\N	WWE Smackdown vs RAW 2 Korea (PS2)	f
1199	fsw10hps2	\N	Full Spectrum Warrior: Ten Hammers (PS2)	f
1200	fsw10hpc	6w2X9m	Full Spectrum Warrior: Ten Hammers (PC)	f
1201	fsw10hps2kor	\N	Full Spectrum Warrior: Ten Hammers (Korea, PS2)	f
1202	fsw10hps2pal	\N	Full Spectrum Warrior: Ten Hammers (PAL, PS2)	f
1203	swbfront2pc	\N	Star Wars Battlefront 2 PC	f
1204	swbfront2ps2	\N	Star Wars Battlefront 2 (PS2)	f
1205	swbfront2ps2j	\N	Star Wars Battlefront 2 (PS2) Japanese	f
1206	worms4d	Bs28Kl	Worms 4 Mayhem Demo	f
1207	whammer40kwa	\N	Warhammer 40,000: Winter Assault	f
1208	whammer40kwaam	\N	Warhammer 40,000: Winter Assault (Automatch)	f
1209	codbigredps2	\N	Call of Duty 2: Big Red One (PS2)	f
1210	dsnattest	\N	ds nat test	f
1212	xmenlegps2pal	\N	X-Men Legends PAL (PS2)	f
1213	xmenlegps2pals	\N	X-Men Legends PAL Spanish (PS2)	f
1215	gbromeam	\N	Great Battles of Rome (Automatch)	f
1216	pbfqm2	\N	PlanetBattlefield QuickMatch 2	f
1217	wsopps2am	\N	World Series of Poker (PS2) (Automatch)	f
1218	wsoppcam	\N	World Series of Poker (PC) (Automatch)	f
1223	vietcong2d	\N	Vietcong 2 Demo	f
1224	eearth2xp1	h3C2jU	Empire Earth II: The Art of Supremacy	f
1225	afllive05ps2	\N	AFL Live 2005 (ps2)	f
1226	fordvchevyps2	\N	Ford Versus Chevy (PS2)	f
1227	hotpacificpcd	\N	Heroes of the Pacific PC Demo	f
1228	hoodzps2	\N	Hoodz (PS2)	f
1229	swbfront2pcb	\N	Star Wars Battlefront 2 PC Beta	f
1230	swbfront2pcd	\N	Star Wars Battlefront 2 PC Demo	f
1231	rtrooperps2	\N	Rogue Trooper (PS2)	f
1233	fswps2jp	\N	Full Spectrum Warrior (PS2, Japanese)	f
1234	and1sballps2	\N	AND1: Streetball Online (PS2)	f
1236	swempiream	\N	Star Wars: Empire at War (Automatch)	f
1238	mariokartds	\N	Mario Kart (DS)	f
1239	genetrooperpc	\N	Gene Trooper (PC)	f
1240	genetrooperps2	\N	Gene Troopers (PS2)	f
1241	legionarena	\N	Legion Arena	f
1242	kott2pc	\N	Knights of the Temple 2 (PC)	f
1243	kott2ps2	\N	Knights of the Temple 2 (PS2)	f
1244	hardtruck	\N	Hard Truck Tycoon	f
1245	wracing2	\N	World Racing 2 (PC)	f
1246	wsoppsp	\N	World Series of Poker (PSP)	f
1247	wsoppspam	\N	World Series of Poker (PSP) (Automatch)	f
1248	infectedpsp	\N	Infected (PSP)	f
1249	infectedpspam	\N	Infected (PSP) (Automatch)	f
1251	unavailable	\N	Test for disabled games	f
1252	tempunavail	\N	Test for temporarily disabled games	f
1253	betonsoldierd	\N	Bet On Soldier	f
1254	ghpballps2	\N	Greg Hastings Paintball (PS2)	f
1255	flatout	\N	FlatOut	f
1256	bfield2xp1	\N	Battlefield 2: Special Forces	f
1257	vietcong2pd	\N	Vietcong 2 Public Demo	f
1258	thawds	\N	Tony Hawks American Wasteland (DS)	f
1259	acrossingds	\N	Animal Crossing (DS)	f
1260	coteaglessp	\N	War Front: Turning Point (Singleplayer)	f
1261	and1sballps2am	\N	AND1: Streetball Online (PS2) (Automatch)	f
1262	mariokartdsam	\N	Mario Kart (DS, Automatch)	f
1264	acrossingdsam	\N	Animal Crossing (DS, Automatch)	f
1265	xmenleg2psp	\N	X-Men: Legends 2 (PSP)	f
1266	lotrbme2	\N	Lord of the Rings: The Battle for Middle-earth 2 (Beta)	f
1267	shatteredunion	\N	Shattered Union	f
1268	serioussam2d	\N	Serious Sam 2 Demo	f
1269	bllrs2005ps2	\N	NBA Ballers 2005 (PS2)	f
1270	bllrs2005ps2d	\N	NBA Ballers 2005  Demo (PS2)	f
1272	mprimeds	\N	Metroid Prime Hunters (DS)	f
1273	racedriver3pc	\N	Race Driver 3 (PC)	f
1274	racedriver3pcd	\N	Race Driver 3  Demo (PC)	f
1275	scsdw	\N	S.C.S. Dangerous Waters	f
1276	scsdwd	\N	S.C.S. Dangerous Waters Demo	f
1277	uchaosrrps2	\N	Urban Chaos: Riot Response (PS2)	f
1278	uchaosrrps2am	\N	Urban Chaos: Riot Response  Automatch (PS2)	f
1280	rdriver3ps2	\N	Race Driver 3 (PS2)	f
1281	rdriver3ps2d	\N	Race Driver 3  Demo (PS2)	f
1282	wptps2pal	\N	World Poker Tour PAL (PS2)	f
1283	rtrooperpc	\N	Rogue Trooper (PC)	f
1284	rtrooperpcam	\N	Rogue Trooper  Automatch (PC)	f
1285	bf2sttest	\N	Battlefield 2 Snapshot testing	f
1289	dsnattest2	\N	ds nat test 2	f
1290	mxun05pc	\N	MX vs. ATV Unleashed (PC)	f
1291	mxun05pcam	\N	MX vs. ATV Unleashed  Automatch (PC)	f
1292	quake4	\N	Quake 4	f
1293	paraworld	\N	ParaWorld	f
1294	paraworldam	\N	ParaWorld Automatch	f
1295	paraworldd	\N	ParaWorld Demo	f
1296	callofduty2	\N	Call of Duty 2	f
1297	bfield1942ps2am	\N	Battlefield Modern Combat  Automatch (PS2)	f
1298	slugfest06ps2	\N	Slugfest '06 (PS2)	f
1299	bleachds	\N	Bleach (DS)	f
1300	lostmagicds	\N	Lost Magic (DS)	f
1301	wofor	\N	WOFOR: War on Terror	f
1302	woforam	\N	WOFOR: War on Terror Automatch	f
1303	woford	\N	WOFOR: War on Terror Demo	f
1305	wofordam	\N	WOFOR: War on Terror Demo Automatch	f
1306	Happinuds	\N	Happinuvectorone! (DS)	f
1307	thawpc	\N	Tony Hawk's American Wasteland (PC)	f
1308	ysstrategyds	\N	Y's Strategy (DS)	f
1309	marvlegps2	\N	Marvel Legends (PS2)	f
1310	marvlegps2am	\N	Marvel Legends  Automatch (PS2)	f
1311	marvlegpsp	\N	Marvel Legends (PSP, PAL)	f
1312	marvlegpspam	\N	Marvel Legends  Automatch (PSP, PAL)	f
1313	marvlegpc	\N	Marvel Legends (PC)	f
1314	marvlegpcam	\N	Marvel Legends  Automatch (PC)	f
1315	marvlegpcd	\N	Marvel Legends  Demo (PC)	f
1316	marvlegpcdam	\N	Marvel Legends Demo Automatch (PC)	f
1317	hustleps2	\N	Hustle: Detroit Streets (PS2)	f
1318	hustleps2am	\N	Hustle: Detroit Streets  Automatch (PS2)	f
1320	koshien2ds	\N	PowerPro Pocket Koshien 2 (DS)	f
1321	lotrbme2r	\N	Lord of the Rings: The Battle for Middle-earth 2	f
1322	tenchuds	\N	Tenchu (DS)	f
1323	contactds	\N	Contact JPN (DS)	f
1324	stella	\N	Battlefield 2142	f
1325	stellad	\N	Battlefield 2142 (Demo)	f
1326	runefactoryds	\N	Rune Factory (DS)	f
1327	tetrisds	\N	Tetris DS (DS)	f
1328	motogp4ps2	\N	MotoGP 4 (PS2)	f
1329	actofwarht	\N	Act of War: High Treason	f
1330	actofwarhtam	\N	Act of War: High Treason Automatch	f
1331	actofwarhtd	\N	Act of War: High Treason Demo	f
1332	actofwarhtdam	\N	Act of War: High Treason Demo Automatch	f
1333	Customrobods	\N	Custom Robo DS (DS)	f
1334	comrade	\N	Comrade	f
1335	greconawf	\N	Ghost Recon: Advanced Warfighter	f
1336	greconawfd	\N	Ghost Recon: Advanced Warfighter Demo	f
1337	asobids	\N	Asobi Taizen (DS)	f
1338	timeshift	\N	TimeShift (PC)	f
1339	timeshiftb	\N	TimeShift Beta (PC)	f
1340	scsdws	\N	S.C.S. Dangerous Waters Steam	f
1341	ffurtdriftps2	\N	The Fast and the Furious: Tokyo Drift (PS2)	f
1342	ffurtdriftps2am	\N	The Fast and the Furious: Tokyo Drift  Automatch (PS2)	f
1344	pokemondpds	\N	Pokemon Diamond-Pearl (DS)	f
1345	coteaglesam	\N	War Front: Turning Point Automatch	f
1346	facesofwar	\N	Faces of War	f
1347	facesofwaram	\N	Faces of War Automatch	f
1348	facesofward	\N	Faces of War Demo	f
1349	bombermanslds	\N	Bomberman Story/Land DS	f
1350	fherjwkk	\N	Namco Test	f
1351	tiumeshiftu	\N	TimeShift (Unlock codes)	f
1352	digistoryds	\N	Digimon Story (DS)	f
1353	touchpanicds	\N	Touch Panic (DS)	f
1354	SampAppTest	\N	Sample App Developement	f
1355	SampAppTestam	\N	Sample App Developement Automatch	f
1358	fearxp1	\N	FEAR: Extraction Point	f
1359	narutorpg3ds	\N	Naruto RPG 3 (DS)	f
1360	digistorydsam	\N	Digimon Story  Automatch (DS)	f
1361	redorchestra	\N	Red Orchestra Ostfront	f
1362	airwingsds	\N	Air Wings (DS)	f
1363	openseasonds	\N	OpenSeason DS (DS)	f
1364	mageknight	\N	Mage Knight Apocalypse	f
1366	mageknightd	\N	Mage Knight Apocalypse Demo	f
1367	starfoxds	\N	Starfox DS (DS)	f
1368	rockmanwds	\N	Rockman WAVES (DS)	f
1369	medieval2	\N	Medieval 2 Total War	f
1370	medieval2am	\N	Medieval 2 Total War Automatch	f
1371	taisends	\N	Sangokushi Taisen DS (DS)	f
1372	mkarmps2	\N	Mortal Kombat: Armageddon (PS2)	f
1373	thps3pcr	\N	Tony Hawk 3 PC (Rerelease)	f
1374	mmvdkds	\N	Mini Mario vs Donkey Kong (DS)	f
1375	ffantasy3ds	\N	Final Fantasy III (DS)	f
1376	marvlegps2p	\N	Marvel Legends PAL (PS2)	f
1377	marvlegps2pam	\N	Marvel Legends  Automatch PAL (PS2)	f
1378	c5	\N	Conflict: Denied Ops	f
1379	rfberlin	\N	Rush for Berlin	f
1380	swat4xp1_tmp	\N	SWAT 4: The Stetchkov Syndicate Temp	f
1381	swordots	\N	Sword of the Stars	f
1382	mahjongkcds	\N	Mah-Jong Kakuto Club (DS)	f
1383	whammermok	\N	Warhammer: Mark of Chaos (OLD)	f
1386	Nushizurids	\N	Nushizuri DS Yama no megumi Kawa no seseragi	f
1387	civ4wrld	oQ3v8V	Civilization IV: Warlords	f
1388	civ4wrldam	oQ3v8V	Civilization IV: Warlords Automatch	f
1389	dsiege2bw	\N	Dungeon Siege II: Broken World	f
1390	blic2007	\N	Brian Lara International Cricket 2007	f
1391	nwn2	\N	NeverWinter Nights 2	f
1392	pssake	\N	Professional Services Sake Test	f
1393	gmtestcd	\N	Test (Chat CD Key validation)	f
1394	gmtestcdam	\N	Test  Automatch (Chat CD Key validation)	f
1395	yugiohgx2ds	\N	Yu-Gi-OH! Duel Monsters GX2 (DS)	f
1396	whammermoc	\N	Warhammer: Mark of Chaos	f
1397	whammermocam	\N	Warhammer: Mark of Chaos Automatch	f
1398	whammermocd	\N	Warhammer: Mark of Chaos Demo	f
1399	flatout2ps2	\N	FlatOut 2 (PS2)	f
1400	cruciform	\N	Genesis Rising: The Universal Crusade	f
1401	blkhwkdntsps2	\N	Delta Force: Black Hawk Down - Team Sabre (PS2)	f
1402	socelevends	\N	World Soccer Winning Eleven DS (DS)	f
1403	konductrads	\N	Konductra (DS)	f
1404	strongholdl	\N	Stronghold Legends	f
1405	wsc2007ps2	\N	World Snooker Championship 2007 (PS2)	f
1406	wsc2007ps3	\N	World Snooker Championship 2007 (PS3)	f
1407	ninsake	\N	Nintendo Sake Test	f
1408	dwctest	\N	DWC NintendoTest App	f
1409	FieldOps	\N	Field Ops	f
1410	wcpoker2pc	\N	World Championship Poker 2 (PC)	f
1411	whammer40kdc	\N	Warhammer 40,000: Dark Crusade	f
1412	whammer40kdcam	\N	Warhammer 40,000: Dark Crusade Automatch	f
1413	fullautops3	\N	Full Auto 2: Battlelines (PS3)	f
1414	civ4jp	\N	Civiliation IV (Japanese)	f
1415	civ4jpam	\N	Civiliation IV  Automatch (Japanese)	f
1416	contactusds	\N	Contact US (DS)	f
1417	thps4pcr	\N	Tony Hawk: Pro Skater 4 (PC) Rerelease	f
1418	thps4pcram	\N	Tony Hawk: Pro Skater 4  Automatch (PC) Rerelease	f
1419	bf2ddostest	\N	Battlefield 2 DDoS testing	f
1420	flatout2pc	GtGLyx	FlatOut 2 (PC)	f
1421	smrailroads	\N	Sid Meier's Railroads!	f
1422	cc3tibwars	\N	Command & Conquer 3: Tiberium Wars	f
1423	topspin2pc	\N	Top Spin 2 (PC)	f
1424	thdhilljamds	\N	Tony Hawk's Downhill Jam (DS)	f
1425	aoex	\N	Age of Empires Expansion	f
1426	rafcivatwart	\N	Rise And Fall: Civilizations at War Test	f
1427	rafcivatwartam	\N	Rise And Fall: Civilizations at War Test Automatch	f
1428	bokujomonods	\N	Bokujo Monogatari DS2: Wish-ComeTrue Island (DS)	f
1429	tothrainbowds	\N	TOTH Rainbow Trail of Light (DS)	f
1430	mkarmpalps2	\N	Mortal Kombat: Armageddon PAL (PS2)	f
1431	preyd	\N	Prey Demo	f
1432	prey	\N	Prey	f
1433	jumpsstars2ds	\N	Jump Super Stars 2 (DS)	f
1434	anno1701	Xa6zS3	Anno 1701	f
1435	civ4ru	\N	Civiliation IV (Russian)	f
1436	civ4ruam	\N	Civiliation IV  Automatch (Russian)	f
1437	civ4ch	\N	Civiliation IV (Chinese)	f
1438	civ4cham	\N	Civiliation IV  Automatch (Chinese)	f
1439	cricket2007	\N	Brian Lara International Cricket 2007	f
1440	eternalforces	\N	Eternal Forces Demo	f
1441	eternalforcesam	\N	Eternal Forces Automatch	f
1442	eforcesr	\N	Eternal Forces	f
1443	bandbrosds	\N	Daiggaso! Band Brothers DX (DS)	f
1444	ptacticsds	\N	Panzer Tactics (DS)	f
1445	tankbeatds	\N	Tank Beat (JPN) (DS)	f
1446	mdungeonds	\N	Mysterious Dungeon: Shiren the Wanderer DS (DS)	f
1447	dqmonjokerds	\N	Dragon Quest Monsters: Joker (DS)	f
1448	draculagolds	\N	Akumajou Dracula: Gallery of Labyrinth (DS)	f
1449	oishiids	\N	Oishii Recipe (DS)	f
1450	stlegacy	\N	Star Trek: Legacy	f
1451	NN2Simple	\N	NatNeg2 Simple Test	f
1452	yakumands	\N	Yakuman DS (DS)	f
1453	marveltcardds	\N	Marvel Trading Card Game (DS)	f
1454	ffantasy3usds	\N	Final Fantasy III - US (DS)	f
1457	testdriveu	\N	Test Drive Unlimited (Unused)	f
1458	test071806	\N	Test	f
1459	chocobombds	\N	Chocobo & Magic Book (DS)	f
1460	puyopuyods	\N	Puyo Puyo! (DS)	f
1461	otonatrainds	\N	Imasara hitoniwa kikenai Otona no Jyoshikiryoku Training DS (DS)	f
1462	luckystar2ds	\N	Lucky Star 2 (DS)	f
1463	lotrbme2wk	\N	Lord of the Rings: The Battle for Middle-earth 2 (Rise of the Witch-King Expansion Pack)	f
1464	crysis	ZvZDcL	Crysis (PC)	f
1465	crysisd	ZvZDcL	Crysis Demo	f
1466	monsterfarmds	\N	Monster Farm DS (DS)	f
1467	naruto5ds	\N	NARUTO: Saikyou Ninja Daikesshuu 5 (DS)	f
1468	picrossds	\N	Picross (DS)	f
1469	wh40kp	\N	Warhammer 40,000: Dawn of War Patch	f
1470	wh40kwap	\N	Warhammer 40,000: Winter Assault Patch	f
1471	digiwrldds	\N	Digimon World DS (DS)	f
1472	pandeponds	\N	Panel De Pon DS (DS)	f
1473	moritashogids	\N	Morita Shogi DS (DS)	f
1474	wormsow2ds	\N	Worms Open Warfare 2 (DS)	f
1475	lithdev	\N	Monolith Development	f
1476	lithdevam	\N	Monolith Development Automatch	f
1477	bf2142	\N	Battlefield 2142	f
1478	bf2142b	\N	Battlefield 2142 (Beta)	f
1479	marvlegps3	\N	Marvel Legends (PS3)	f
1480	marvlegps3am	\N	Marvel Legends  Automatch (PS3)	f
1481	marvlegps3p	\N	Marvel Legends PAL (PS3)	f
1482	marvlegps3pam	\N	Marvel Legends PAL  Automatch (PS3)	f
1483	paradisecity	\N	Paradise City	f
1484	whammermocdam	\N	Warhammer: Mark of Chaos Demo Automatch	f
1485	djangosabds	\N	Bokura No Taiyou: Django & Sabata  (DS)	f
1488	tqexp1	\N	Titan Quest: Immortal Throne	f
1489	tqexp1am	\N	Titan Quest: Immortal Throne (Automatch)	f
1492	marveltcard	\N	Marvel Trading Card Game (PC & PSP)	f
1493	marveltcardps	\N	Marvel Trading Card Game (PSP)	f
1494	heroesmanads	\N	Seiken Densetsu: Heroes of Mana (DS)	f
1495	prismgs	\N	PRISM: Guard Shield	f
1496	prismgsd	\N	PRISM: Guard Shield Demo	f
1497	testdriveub	\N	Test Drive Unlimited	f
1498	testdriveud	\N	Test Drive Unlimited Demo	f
1499	swempirexp1	\N	Star Wars: Empire at War - Forces of Corruption	f
1500	dsakurads	\N	Dragon Sakura DS (DS)	f
1501	gopetsvids	\N	GoPets: Vacation Island (DS)	f
1502	kurikinds	\N	Kurikin (DS)	f
1503	codedarmspsp	\N	Coded Arms (PSP)	f
1504	codedarmspspam	\N	Coded Arms  Automatch (PSP)	f
1505	wiinat	\N	Wii NAT Negotiation Testing	f
1506	whtacticspsp	\N	Warhammer 40,000: Tactics (PSP)	f
1507	whtacticspspam	\N	Warhammer 40,000: Tactics  Automatch (PSP)	f
1508	armedass	\N	ArmA	f
1509	ffcryschronds	\N	Final Fantasy: Crystal Chronicles - Ring of Fate (DS)	f
1510	preystarsds	\N	Prey The Stars (DS)	f
1511	bleach2ds	\N	Bleach DS 2: Requiem in the black robe (DS)	f
1512	marvlegnpsp	\N	Marvel Legends (PSP, NTSC)	f
1513	marvlegnpspam	\N	Marvel Legends  Automatch (PSP, NTSC)	f
1514	spartaaw	\N	Sparta: Ancient Wars	f
1515	spartaawd	\N	Sparta: Ancient Wars Demo	f
1516	civ4mac	CWiCbk	Civilization IV (MAC)	f
1517	civ4macam	CWiCbk	Civilization IV  Automatch (MAC)	f
1518	civ4wrldmac	QtCpWE	Civilization IV: Warlords (MAC)	f
1519	civ4wrldmacam	QtCpWE	Civilization IV: Warlords  Automatch (MAC)	f
1520	pokebattlewii	\N	Pokemon Battle Revolution (Wii)	f
1521	puzquestds	\N	Puzzle Quest: Challenge of the Warlords (DS)	f
1522	doraemonds	\N	Doraemon Nobita no Shinmakai Daiboken DS (DS)	f
1523	eearth3	\N	Empire Earth III	f
1524	eearth3d	\N	Empire Earth III  Demo	f
1525	cc3tibwarsmb	\N	Command & Conquer 3: Tiberium Wars Match Broadcast	f
1527	bf2142d	\N	Battlefield 2142 Demo	f
1528	anno1701d	taEf7n	Anno 1701 Demo	f
1529	medieval2d	\N	Medieval II Demo	f
1530	civ4wrldjp	oQ3v8V	Civilization IV: Warlords (Japan)	f
1531	civ4wrldjpam	oQ3v8V	Civilization IV: Warlords  Automatch (Japan)	f
1532	civ4wrldcn	oQ3v8V	Civilization IV: Warlords (Chinese)	f
1533	civ4wrldcnam	oQ3v8V	Civilization IV: Warlords  Automatch (Chinese)	f
1534	whammer40ktds	\N	Warhammer 40,000 Tactics (DS)	f
1535	mukoubuchids	\N	Mukoubuchi - Goburei, Shuryoudesune (DS)	f
1536	cusrobousds	\N	Gekitoh! Custom Robo (DS) (US)	f
1537	kurikurimixds	\N	Kuri Kuri Mix DS (DS)	f
1538	custoboeuds	\N	Custom Robo (EU) (DS)	f
1539	whammermoct	\N	Warhammer: Mark of Chaos Test	f
1540	whammermoctam	\N	Warhammer: Mark of Chaos Test Automatch	f
1541	sweawfocd	\N	Forces of Corruption Demo	f
1542	aoe3wcd	\N	Age of Empires 3: The Warchiefs Demo	f
1543	tolmamods	\N	Tolmamo (DS)	f
1544	patchtest	\N	Patching Test	f
1545	dkracingds	\N	Diddy Kong Racing DS (DS)	f
1546	fullautops3d	\N	Full Auto 2: Battlelines  Demo (PS3)	f
1547	themark	\N	The Mark	f
1548	themarkam	\N	The Mark Automatch	f
1549	bf2142e	\N	Battlefield 2142 (EAD)	f
1550	supcommb	\N	Supreme Commander (Beta)	f
1551	dow_dc	\N	Dawn of War: Dark Crusade	f
1552	fuusuibands	\N	Fuusuiban (DS)	f
1553	sweawfoc	\N	Star Wars: Empire at War - Forces of Corruption	f
1554	s_darkmmm	\N	Dark Messiah of Might and Magic	f
1555	ppirates	\N	Puzzle Pirates	f
1556	mschargedwii	\N	Mario Strikers Charged (Wii)	f
1557	8ballstarsds	\N	8-Ball Allstars (DS)	f
1558	tmntds	\N	Teenage Mutant Ninja Turtles (DS)	f
1559	surfsupds	\N	Surf's Up (DS)	f
1560	elemonsterds	\N	Elemental Monster (DS)	f
1561	cc3tibwarsam	\N	Command & Conquer 3: Tiberium Wars Automatch	f
1562	picrossEUds	\N	Picross (EU) (DS)	f
1563	greconawf2	qvOwuX	Ghost Recon: Advanced Warfighter 2	f
1564	greconawf2am	\N	Ghost Recon: Advanced Warfighter 2 Automatch	f
1565	greconawf2d	\N	Ghost Recon: Advanced Warfighter 2 Demo	f
1566	yugiohWC07ds	\N	Yu-Gi-Oh! Dual Monsters World Championship 2007 (DS)	f
1567	tgmasterds	\N	Table Game Master DS (DS)	f
1568	batwars2wii	\N	Battalion Wars II (Wii)	f
1569	Doragureidods	\N	Doragureido (DS)	f
1570	armedassd	\N	ArmA Demo	f
1571	ffantasy3euds	\N	Final Fantasy III - EU (DS)	f
1572	rockstardev	\N	Rockstar Development	f
1573	rockstardevam	\N	Rockstar Development Automatch	f
1575	mparty1ds	\N	Mario Party (DS)	f
1576	stalkerscd	\N	STALKER: Shadows of Chernobyl Beta	f
1577	swempiremac	\N	Star Wars: Empire at War (Mac)	f
1578	swempiremacam	\N	Star Wars: Empire at War  Automatch (Mac)	f
1579	marvlegjpps3	\N	Marvel Legends (PS3, Japan)	f
1580	marvlegjpps3am	\N	Marvel Legends  Automatch (PS3, Japan)	f
1581	drmariowii	\N	Dr. Mario (WiiWare)	f
1582	springwidgets	\N	Spring Widgets	f
1583	springwidgetsam	\N	Spring Widgets Automatch	f
1584	lotrbfme2	\N	The Rise of The Witch-king	f
1585	wmarkofchaos	\N	Warhammer Mark of Chaos	f
1586	warmonger	\N	Warmonger	f
1587	everquest2	\N	EverQuest II	f
1588	startreklegacy	\N	Star Trek: Legacy	f
1589	freessbalpha	\N	Freestyle Street Basketball Client Alpha	f
1590	lozphourds	\N	The Legend of Zelda: Phantom Hourglass (DS)	f
1591	vanguardbeta	\N	Vanguard beta	f
1592	digisunmoonds	\N	Digimon Story Sunburst/Moonlight (DS)	f
1593	wmarkofchaosd	\N	Warhammer Mark of Chaos Demo	f
1594	cruciformam	\N	Genesis Rising: The Universal Crusade Automatch	f
1595	tcghostreconaw	\N	tcghostreconaw	f
1596	ghostraw	\N	Ghost Recon Advanced Warfighter	f
1597	rainbowsixv	\N	Rainbow Six Vegas	f
1598	wmarkofchaosdam	\N	Warhammer Mark of Chaos Demo Automatch	f
1599	crysisb	\N	Crysis Beta	f
1600	scotttest	\N	Scott's test gamename	f
1601	rftbomb	\N	Rush for the Bomb	f
1602	motogp2007	\N	MotoGP 2007	f
1603	motogp2007am	\N	MotoGP 2007 Automatch	f
1604	cityofheroes	\N	City of Heroes	f
1605	cityofvl	\N	City of Villains	f
1606	titanquestit	\N	Titan Quest Immortal Throne	f
1607	girlsds	\N	Girls (DS)	f
1608	mariokartkods	\N	Mario Kart DS (DS) (KOR)	f
1609	mmessagesds	\N	Mixed Messages (DS)	f
1610	topanglerwii	\N	Top Angler (Wii)	f
1611	swbfffpsp	\N	Star Wars Battlefront: Renegade Squadron (PSP)	f
1612	facesow	\N	Faces of War	f
1615	dexplorerds	\N	Dungeon Explorer (DS)	f
1616	civconps3	\N	Civilization Revolution (PS3)	f
1617	civconps3am	\N	Civ Console  Automatch (PS3)	f
1618	civconps3d	\N	Civilization Revolution Demo (PS3)	f
1620	stalkerscb	\N	STALKER: Shadows of Chernobyl Beta (Unused)	f
1622	secondlife	\N	Second Life	f
1623	tdubeta	\N	Test Drive Unlimited Beta	f
1624	elevenkords	\N	World Soccer Winning Eleven DS (KOR) (DS)	f
1625	bsmidway	\N	Battlestations Midway Demo	f
1626	etforces	\N	Eternal Forces	f
1627	genesisrbeta	\N	Genesis Rising Beta	f
1628	tankbeatusds	\N	Tank Beat (US) (DS)	f
1629	champgamesps3	\N	High Stakes on the Vegas Strip: Poker Edition (PS3)	f
1631	eearth3b	\N	Empire Earth III Beta	f
1632	eearth3am	\N	Empire Earth III  Automatch	f
1633	chocmbeuds	\N	Chocobo & Magic Book (EU) (DS)	f
1634	supcommdemo	\N	Supreme Commander Demo	f
1635	tetriskords	\N	Tetris DS (KOR) (DS)	f
1636	jissenpachwii	\N	Jissen Pachinko Slot (Wii)	f
1637	wincircleds	\N	Winner's Circle (DS)	f
1638	itadakistds	\N	Itadaki Street DS (DS)	f
1639	smrailroadsjp	\N	Sid Meier's Railroads! Japan	f
1640	smrailroadsjpam	\N	Sid Meier's Railroads! Japan Automatch	f
1641	megamansfds	\N	Mega Man Star Force (US) (DS)	f
1642	facesofwarxp1	\N	Faces of War Standalone (XP1)	f
1643	facesofwarxp1am	\N	Faces of War Standalone  Automatch (XP1)	f
1644	timeshiftx	\N	TimeShift (Xbox 360)	f
1645	timeshiftps3	\N	TimeShift (PS3)	f
1646	powerpinconds	\N	Powershot Pinball Constructor (DS)	f
1647	kingbeetlesds	\N	The King of Beetles Mushiking Super Collection (DS)	f
1648	dragonbzwii	\N	Dragonball Z (Wii)	f
1649	atlas_samples	\N	ATLAS Sample Apps	f
1650	cc3tibwarsd	\N	Command & Conquer 3 Demo	f
1651	supcomm	\N	Supreme Commander	f
1652	assaultheroes	\N	Assault Heroes	f
1653	assaultheroesam	\N	Assault Heroes Automatch	f
1654	pokedungeonds	\N	Pokemon Fushigi no Dungeon (DS)	f
1655	PowaPPocketds	\N	PowaProkun Pocket10 (DS)	f
1656	GunMahjongZds	\N	Kidou Gekidan Haro Ichiza Gundam Mah-jong+Z (DS)	f
1657	fullmatcgds	\N	Fullmetal Alchemist Trading Card Game (DS)	f
1658	smashbrosxwii	\N	Dairantou Smash Brothers X (Wii)	f
1659	disfriendsds	\N	Disney Friends DS (DS)	f
1660	Jyotrainwii	\N	Minna de Jyoshiki Training Wii (Wii)	f
1661	roguewarpc	\N	Rogue Warrior (PC)	f
1662	roguewarpcam	\N	Rogue Warrior  Automatch (PC)	f
1663	roguewarpcd	\N	Rogue Warrior  Demo (PC)	f
1664	roguewarps3	\N	Rogue Warrior (PS3)	f
1665	roguewarps3am	\N	Rogue Warrior  Automatch (PS3)	f
1666	roguewarps3d	\N	Rogue Warrior  Demo (PS3)	f
1667	archlord	\N	Archlord	f
1668	racedriverds	\N	Race Driver (DS)	f
1669	kaihatsuds	\N	Kaihatsushitsu (DS)	f
1670	ardinokingds	\N	Ancient Ruler Dinosaur King (DS)	f
1671	redbaronww1	\N	Red Baron WWI	f
1672	greconawf2b	\N	Ghost Recon: Advanced Warfighter 2 Beta	f
1673	greconawf2bam	\N	Ghost Recon: Advanced Warfighter 2 Beta Automatch	f
1674	ubisoftdev	\N	Ubisoft Development	f
1675	ubisoftdevam	\N	Ubisoft Development Automatch	f
1676	testdriveuak	\N	Test Drive Unlimited (Akella)	f
1677	armaas	\N	ArmA: Armed Assault	f
1678	gta4ps3	\N	Grand Theft Auto 4 (PS3)	f
1679	gta4ps3am	\N	Grand Theft Auto 4  Automatch (PS3)	f
1680	gta4pc	\N	Grand Theft Auto 4 (PC)	f
1681	gta4pcam	\N	Grand Theft Auto 4  Automatch (PC)	f
1682	flashanzands	\N	Flash Anzan Doujou (DS)	f
1683	gta4x	\N	Grand Theft Auto 4 (Xbox 360)	f
1684	gta4xam	\N	Grand Theft Auto 4  Automatch (Xbox 360)	f
1685	pokerangerds	\N	Pokemon Ranger 2 (DS)	f
1686	megamansfeuds	\N	Mega Man Star Force (EU) (DS)	f
1687	mariokartwii	\N	Mario Kart Wii (Wii)	f
1688	swtakoronwii	\N	Shall we Takoron (Wii)	f
1689	phylon	\N	Phylon	f
1690	wsc2007pc	\N	World Snooker Championship 2007 (PC)	f
1691	warfronttp	\N	War Front: Turning Point	f
1692	fearxp2	\N	FEAR Perseus Mandate (PC)	f
1693	risingeaglepc	\N	Rising Eagle	f
1694	bombls2ds	\N	Touch! Bomberman Land 2 / Bomberman DS 2 (DS)	f
1695	momoden16wii	\N	Momotaro Dentetsu 16 - Hokkaido Daiido no Maki! (Wii)	f
1696	sonriders2wii	\N	Sonic Riders 2 (Wii)	f
1697	sonicrushads	\N	Sonic Rush Adventure (DS)	f
1698	ghostsquadwii	\N	Ghost Squad (Wii)	f
1699	runefantasyds	\N	Rune Factory: A Fantasy Harvest Moon (DS)	f
1700	contrads	\N	Contra DS (DS)	f
1701	bleach1USds	\N	Bleach DS (US) (DS)	f
1702	bleach2USds	\N	Bleach DS 2 (US) (DS)	f
1703	civ4bts	Cs2iIq	Civilization IV: Beyond the Sword	f
1704	civ4btsam	Cs2iIq	Civilization IV: Beyond the Sword Automatch	f
1705	eearth3bam	\N	Empire Earth III Beta Automatch	f
1706	eearth3dam	\N	Empire Earth III  Demo Automatch	f
1707	thetsuriwii	\N	The Tsuri (Wii)	f
1708	theracewii	\N	The Race (Wii)	f
1709	tankbeat2ds	\N	Tank Beat 2 (DS)	f
1710	onslaughtpc	\N	Onslaught: War of the Immortals	f
1711	onslaughtpcam	\N	Onslaught: War of the Immortals Automatch	f
1712	onslaughtpcd	\N	Onslaught: War of the Immortals Demo	f
1713	jikkyoprowii	\N	Jikkyo Powerful Pro Yakyu Wii (Wii)	f
1714	cc3dev	\N	Command & Conquer 3 Dev Environment	f
1715	cc3devam	\N	Command & Conquer 3 Dev Environment Automatch	f
1716	wsc2007	\N	World Snooker Championship 2007	f
1717	luminarcUSds	\N	Luminous Arc (US) (DS)	f
1718	bleach1EUds	\N	Bleach DS (EU) (DS)	f
1719	MSolympicds	\N	Mario & Sonic at the Olympic Games (DS)	f
1720	keuthendev	\N	Keuthen.net Development	f
1721	keuthendevam	\N	Keuthen.net Development Automatch	f
1722	mclub4ps3	\N	Midnight Club 4 (PS3)	f
1723	mclub4ps3am	\N	Midnight Club 4  Automatch (PS3)	f
1724	mclub4xbox	\N	Midnight Club 4 (Xbox360)	f
1725	mclub4xboxam	\N	Midnight Club 4  Automatch (Xbox360)	f
1726	girlssecretds	\N	Girls Secret Diary (DS)	f
1727	ut3pc	\N	Unreal Tournament 3 (PC)	f
1728	ut3pcam	\N	Unreal Tournament 3  Automatch (PC)	f
1729	ut3pcd	\N	Unreal Tournament 3  Demo (PC)	f
1730	ecorisds	\N	Ecoris (DS)	f
1731	ww2btanks	\N	WWII Battle Tanks: T-34 vs Tiger	f
1732	genesisr	\N	Genesis Rising	f
1733	tpfolpc	\N	Turning Point: Fall of Liberty (PC)	f
1734	tpfolpcam	\N	Turning Point: Fall of Liberty  Automatch (PC)	f
1735	tpfolpcd	\N	Turning Point: Fall of Liberty  Demo (PC)	f
1736	tpfolps3	\N	Turning Point: Fall of Liberty (PS3)	f
1737	tpfolps3am	\N	Turning Point: Fall of Liberty  Automatch (PS3)	f
1738	hsmusicalds	\N	High School Musical (DS)	f
1739	cardheroesds	\N	Card Heroes (DS)	f
1740	metprime3wii	\N	Metroid Prime 3 (Wii)	f
1741	Digidwndskds	\N	Digimon World Dawn/Dusk (DS)	f
1742	worldshiftpc	\N	WorldShift (PC)	f
1743	worldshiftpcam	\N	WorldShift  Automatch (PC)	f
1744	worldshiftpcd	\N	WorldShift  Demo (PC)	f
1745	kingclubsds	\N	King of Clubs (DS)	f
1746	MSolympicwii	\N	Mario & Sonic at the Olympic Games (Wii)	f
1747	ingenious	\N	Ingenious	f
1748	potco	\N	Pirates of the Caribbean Online	f
1749	madden08ds	\N	Madden NFL 08 (DS)	f
1750	vanguardsoh	\N	Vanguard Saga of Heroes	f
1751	fury	\N	Fury	f
1752	twoods08ds	\N	Tiger Woods 08 (DS)	f
1753	otonazenshuds	\N	Otona no DS Bungaku Zenshu (DS)	f
1754	bestfriendds	\N	Best Friend - Main Pferd (DS)	f
1755	nindev	\N	Nintendo Network Development Testing	f
1756	quakewarset	\N	Enemy Territory: Quake Wars	f
1757	momotarodends	\N	Momotaro Dentetsu 16 ~ Hokkaido Daiido no Maki! (DS)	f
1758	gwgalaxiesds	\N	Geometry Wars Galaxies (DS)	f
1759	gwgalaxieswii	\N	Geometry Wars Galaxies (Wii)	f
1760	hrollerzds	\N	Homies Rollerz (DS)	f
1761	dungeonr	\N	Dungeon Runners	f
1762	dirtdemo	\N	DiRT Demo	f
1763	nameneverds	\N	Nameless Neverland (DS)	f
1764	proyakyuds	\N	Pro Yakyu Famisute DS (DS)	f
1765	foreverbwii	\N	Forever Blue (Wii)	f
1766	ee3alpha	\N	Empire Earth III Alpha	f
1767	rachelwood	\N	Rachel Wood Test Game Name	f
1768	officersgwupc	\N	Officers: God With Us	f
1769	officersgwupcam	\N	Officers: God With Us Automatch	f
1770	swordotnw	\N	Sword of the New World	f
1771	nfsprostds	\N	Need for Speed Pro Street (DS)	f
1772	commandpc	\N	Commanders: Attack!	f
1773	commandpcam	\N	Commanders: Attack! Automatch	f
1774	whamdowfr	\N	Warhammer 40,000: Dawn of War - Soulstorm	f
1775	whamdowfram	\N	Warhammer 40,000: Dawn of War - Final Reckoning Automatch	f
1776	shirenUSEUds	\N	Mysterious Dungeon: Shiren the Wanderer DS (US-EU) (DS)	f
1777	sporeds	\N	Spore (DS)	f
1778	mysecretsds	\N	My Secrets (DS)	f
1779	nights2wii	\N	NiGHTS: Journey of Dreams (Wii)	f
1780	tgstadiumwii	\N	Table Game Stadium (Wii)	f
1781	lovegolfwii	\N	Wii Love Golf (Wii)	f
1782	hookedfishwii	\N	Hooked! Real Motion Fishing (Wii)	f
1783	tankbattlesds	\N	Tank Battles (DS)	f
1784	anarchyonline	\N	Anarchy Online	f
1785	hookedEUwii	\N	Hooked! Real Motion Fishing (EU) (Wii)	f
1786	hookedJPNwii	\N	Hooked! Real Motion Fishing (JPN) (Wii)	f
1787	tankbeatEUds	\N	Tank Beat (EU) (DS)	f
1788	farcry	\N	Far Cry	f
1789	yugiohwc08ds	\N	Yu-Gi-Oh! World Championship 2008 (DS)	f
1790	trackfieldds	\N	International Track & Field (DS)	f
1791	quakewarsetb	\N	Enemy Territory: Quake Wars Beta	f
1792	RKMvalleyds	\N	River King: Mystic Valley (DS)	f
1793	DSwars2ds	\N	DS Wars 2 (DS)	f
1794	cdkeys	\N	CD Key Admin Site Testing	f
1795	wordjongds	\N	Word Jong - US (DS)	f
1796	raymanrr2wii	\N	Rayman Raving Rabbids 2 (Wii)	f
1797	nanostray2ds	\N	Nanostray 2 (DS)	f
1798	guitarh3wii	\N	Guitar Hero 3 (Wii)	f
1799	suddenstrike3	\N	Sudden Strike 3: Arms for Victory	f
1800	segatennisps3	\N	Sega SuperStars Tennis (PS3)	f
1801	segatennisps3am	\N	Sega SuperStars Tennis Automatch	f
1802	dfriendsEUds	\N	Disney Friends DS (EU)	f
1803	fifa08ds	\N	FIFA 08 Soccer (DS)	f
1804	suitelifeds	\N	Suite Life of Zack & Cody: Circle of Spies (DS)	f
1805	dragladeds	\N	Draglade (DS)	f
1806	takoronUSwii	\N	Takoron (US) (Wii)	f
1807	dragonbzUSwii	\N	Dragonball Z: Tenkaichi 3 (US) (Wii)	f
1808	arkanoidds	\N	Arkanoid DS (DS)	f
1809	rfactory2ds	\N	Rune Factory 2 (DS)	f
1810	dow	\N	Dawn of War	f
1811	nitrobikewii	\N	Nitrobike (Wii)	f
1812	bokujyods	\N	Bokujyo Monogatari Himawari Shoto wa Oosawagi! (DS)	f
1813	WSWelevenwii	\N	World Soccer Winning Eleven Wii (Wii)	f
1814	cc3xp1	\N	Command & Conquer 3: Expansion Pack 1	f
1815	cc3xp1am	\N	Command & Conquer 3: Expansion Pack 1 Automatch	f
1816	pachgundamwii	\N	Pachisuro Kido Senshi Gundam Aisenshi Hen (Wii)	f
1817	newgamename	\N	dhdh	f
1818	newgamenameam	\N	dhdh Automatch	f
1819	gsTiaKreisDS	\N	Genso Suikokuden TiaKreis (DS)	f
1820	ultimateMKds	\N	Ultimate Mortal Kombat (DS)	f
1821	MLBallstarsds	\N	Major League Baseball Fantasy All-Stars (DS)	f
1822	wicb	\N	World in Conflict Beta	f
1823	ee3beta	\N	Empire Earth III Beta	f
1824	WSWeleven07ds	\N	World Soccer Winning Eleven DS 2007 (DS)	f
1825	mafia2pc	\N	Mafia 2 (PC)	f
1826	mafia2pcam	\N	Mafia 2  Automatch (PC)	f
1827	mafia2ps3	\N	Mafia 2 (PS3)	f
1828	mafia2ps3am	\N	Mafia 2  Automatch (PS3)	f
1829	chocotokiwii	\N	Chocobo no Fushigina Dungeon: Toki-Wasure no Meikyu (Wii)	f
1830	zeroGds	\N	ZeroG (DS)	f
1831	suitelifeEUds	\N	Suite Life of Zack & Cody: Circle of Spies (EU) (DS)	f
1832	furaishi3wii	\N	Furai no Shiren 3 Karakuri Yashiki no Nemuri Hime (Wii)	f
1833	ben10bb	\N	Ben 10 Bounty Battle	f
1834	ben10bbam	\N	Ben 10 Bounty Battle Automatch	f
1835	risingeagleg	\N	Rising Eagle	f
1836	timeshiftg	\N	Timeshift	f
1837	cadZ2JPwii	\N	Caduceus Z2 (Wii)	f
1838	rockmanBSDds	\N	Rockman 2 - Berserk: Shinobi / Dinosaur (DS)	f
2419	sawpcd	\N	SAW  Demo (PC)	f
1839	THPGds	\N	Tony Hawks Proving Ground (DS)	f
1840	birhhpc	\N	Brothers In Arms: Hell's Highway (PC)	f
1841	birhhpcam	\N	Brothers In Arms: Hell's Highway Automatch (PC)	f
1842	birhhps3	\N	Brothers In Arms: Hell's Highway (PS3)	f
1843	birhhps3am	\N	Brothers In Arms: Hell's Highway Clone Automatch (PS3)	f
1844	sakuraTDDds	\N	Sakura Taisen Dramatic Dungeon - Kimiarugatame (DS)	f
1845	fsxa	\N	Flight Simulator X: Acceleration	f
1846	fsxaam	\N	Flight Simulator X: Acceleration Automatch	f
1847	ee3	\N	Empire Earth 3	f
1848	nwn2mac	\N	NeverWinter Nights 2 (MAC)	f
1849	nwn2macam	\N	NeverWinter Nights 2  Automatch (MAC)	f
1850	greconawf2g	\N	Ghost Recon Advanced Warfighter 2	f
1851	quakewarsetd	\N	Enemy Territory: Quake Wars Demo	f
1852	evosoccer08ds	\N	Pro Evolution Soccer 2008 (DS)	f
1853	cohofbeta	\N	Company of Heroes: Opposing Fronts MP Beta	f
1854	tetrisppwii	\N	Tetris++ (WiiWare)	f
1855	bioshock	\N	Bioshock Demo	f
1856	bioshockd	\N	Bioshock	f
1857	civrevods	\N	Sid Meier's Civilization Revolution (DS)	f
1858	ninjagaidends	\N	Ninja Gaiden: Dragon Sword (DS)	f
1859	MOHADemo	\N	Medal of Honor Airborne Demo	f
1860	clubgameKORds	\N	Clubhouse Games (KOR) (DS)	f
1861	wic	\N	World in Conflict Demo	f
1862	wicd	\N	World in Conflict Demo	f
1863	ut3pcdam	\N	Unreal Tournament 3  Demo  Automatch (PC)	f
1864	evosoc08USds	\N	Pro Evolution Soccer 2008 (US) (DS)	f
1865	wormsasowii	\N	Worms: A Space Oddity (Wii)	f
1866	painkillerod	\N	Painkiller Overdose	f
1867	painkillerodam	\N	Painkiller Overdose Automatch	f
1868	cnpanzers2cw	\N	Codename Panzers 2: Cold Wars (PC)	f
1869	cnpanzers2cwam	\N	Codename Panzers 2: Cold Wars Automatch	f
1870	cnpanzers2cwd	\N	Codename Panzers 2: Cold Wars Demo	f
1871	luminarc2ds	\N	Luminous Arc 2 Will (DS)	f
1872	noahprods	\N	Noah's Prophecy (DS)	f
1873	wiibombmanwii	\N	Wii Bomberman / WiiWare Bomberman / Bomberman Land Wii 2 (Wii)	f
1874	thesactionwii	\N	The Shooting Action (Wii)	f
1875	Asonpartywii	\N	Asondewakaru THE Party/Casino (Wii)	f
1876	sptouzokuds	\N	Steel Princess Touzoku Koujyo (DS)	f
1877	heiseikyods	\N	Heisei Kyoiku Iinkai DS Zenkoku Touitsu Moshi Special (DS)	f
1878	famstadiumwii	\N	Family Stadium Wii (Wii)	f
1879	ut3ps3	\N	Unreal Tournament 3 (PS3)	f
1880	ut3ps3am	\N	Unreal Tournament 3  Automatch (PS3)	f
1881	ut3ps3d	\N	Unreal Tournament 3  Demo (PS3)	f
1882	ecocreatureds	\N	Eco-Creatures: Save the Forest (DS)	f
1883	mohairborne	\N	Medal of Honor: Airborne	f
1884	whamdowfrb	\N	Warhammer 40,000: Dawn of War - Final Reckoning Beta	f
1885	whamdowfrbam	\N	Warhammer 40,000: Dawn of War - Final Reckoning Beta Automatch	f
1886	puyobomberds	\N	Puyo Puyo Bomber (DS)	f
1887	s_tf2	\N	Team Fortress 2	f
1888	painkillerodd	\N	Painkiller Overdose Demo	f
1889	goreAV	\N	Gore (Ad version)	f
1890	goreAVam	\N	Gore  Automatch (Ad version)	f
1891	goreAVd	\N	Gore  Demo (Ad version)	f
1892	fstreetv3ds	\N	FIFA Street v3 (DS)	f
1893	dragladeEUds	\N	Draglade (EU) (DS)	f
1894	culdceptds	\N	Culdcept DS (DS)	f
1895	ffwcbeta	\N	Frontlines: Fuel of War Beta	f
1896	ffowbeta	\N	Frontlines: Fuel of War Beta	f
1897	painkilleroddam	\N	Painkiller Overdose Demo Automatch	f
1901	pkodgerman	\N	Painkiller Overdose (German)	f
1902	pkodgermanam	\N	Painkiller Overdose  Automatch (German)	f
1903	pkodgermand	\N	Painkiller Overdose  Demo (German)	f
1904	cohof	\N	Company of Heroes: Opposing Fronts	f
1905	puzzlernumds	\N	Puzzler Number Placing Fun & Oekaki Logic 2 (DS)	f
1906	supcomfabeta	\N	Supreme Commander: Forged Alliance beta	f
1907	condemned2bs	\N	Condemned 2: Bloodshot (PS3)	f
1908	condemned2bsam	\N	Condemned 2: Bloodshot Automatch	f
1909	condemned2bsd	\N	Condemned 2: Bloodshot Demo (PS3)	f
1910	potbs	\N	Pirates of the Burning Sea	f
1911	mxvsatvutps2	\N	MX vs ATV Untamed (PS2)	f
1912	mxvsatvutps2am	\N	MX vs ATV Untamed  Automatch (PS2)	f
1913	jikkyopprowii	\N	Jikkyo Powerful Pro Yakyu Wii Kettei ban (Wii)	f
1914	ut3demo	\N	Unreal Tournament 3 Demo	f
1915	ut3	\N	Unreal Tournament 3	f
1916	valknightswii	\N	Valhalla Knights (Wii)	f
1917	amfbowlingds	\N	AMF Bowling: Pinbusters! (DS)	f
1918	ducatimotods	\N	Ducati Moto (DS)	f
1919	arkwarriors	\N	Arkadian Warriors	f
1920	arkwarriorsam	\N	Arkadian Warriors Automatch	f
1921	mxvsatvutwii	\N	MX vs ATV Untamed (Wii)	f
1922	cheetah3ds	\N	The Cheetah Girls 3 (DS)	f
1923	whammermocbm	\N	Warhammer: Mark of Chaos - Battle March	f
1924	whammermocbmam	\N	Warhammer: Mark of Chaos - Battle March Automatch	f
1925	whammermocbmd	\N	Warhammer: Mark of Chaos - Battle March Demo	f
1926	linksds	\N	Links (DS)	f
1927	swbfront3pc	\N	Star Wars Battlefront 3 (PC)	f
1928	swbfront3pcam	\N	Star Wars Battlefront 3  Automatch (PC)	f
1929	siextremeds	\N	Space Invaders Extreme	f
1931	redsteel2wii	\N	Red Steel 2 (Wii)	f
1932	gorese	\N	Gore Special Edition	f
1933	dinokingEUds	\N	Ancient Ruler Dinosaur King (EU) (DS)	f
1934	civ4btsjp	Cs2iIq	Civilization IV: Beyond the Sword (Japanese)	f
1935	civ4btsjpam	Cs2iIq	Civilization IV: Beyond the Sword  Automatch (Japanese)	f
1936	charcollectds	\N	Character Collection! DS (DS)	f
1937	timeshiftd	\N	TimeShift  Demo (PC)	f
1938	gtacwarsds	\N	Grand Theft Auto: Chinatown Wars (DS)	f
1939	fireemblemds	\N	Fire Emblem DS (DS)	f
1940	soccerjamds	\N	Soccer Jam (DS)	f
1941	gravitronwii	\N	Gravitronix (WiiWare)	f
1942	mdamiiwalkds	\N	Minna de Aruku! Mii Walk (DS)	f
1943	puzzlemojiwii	\N	Kotoba no Puzzle Moji Pittan Wii (WiiWare)	f
1944	nitrobikeps2	\N	Nitrobike (PS2)	f
1945	nitrobikeps2am	\N	Nitrobike  Automatch (PS2)	f
1946	dinokingUSds	\N	Dinosaur King (US) (DS)	f
1947	harvfishEUds	\N	Harvest Fishing (EU) (DS)	f
1948	harmooniohds	\N	Harvest Moon : Island of Happiness (US) (DS)	f
1949	harmoon2ds	\N	Harvest Moon DS 2 (EU) (DS)	f
1950	sbk08pc	\N	SBK '08: Superbike World Championship (PC)	f
1951	sbk08pcam	\N	SBK '08: Superbike World Championship  Automatch (PC)	f
1952	sbk08pcd	\N	SBK '08: Superbike World Championship  Demo (PC)	f
1953	sbk08ps3	\N	SBK '08: Superbike World Championship (PS3)	f
1954	sbk08ps3am	\N	SBK '08: Superbike World Championship  Automatch (PS3)	f
1955	exitds	\N	Hijyoguchi: EXIT DS (DS)	f
1956	spectrobes2ds	\N	Kaseki Choshinka Spectrobes 2 (DS)	f
1957	nanost2EUds	\N	Nanostray 2 (EU) (DS)	f
1958	crysisspd	\N	Crysis SP Demo	f
1959	evosoc08EUwii	\N	Pro Evolution Soccer 2008 (EU) (Wii)	f
1960	evosoc08USwii	\N	Pro Evolution Soccer 2008 (US) (Wii)	f
1961	sdamigowii	\N	Samba de Amigo (Wii)	f
1962	timeshiftps3d	\N	TimeShift  Demo (PS3)	f
1963	chesswii	\N	Wii Chess (Wii)	f
1964	ecolisEUds	\N	Ecolis (EU) (DS)	f
1965	rdgridds	\N	Race Driver: Grid (DS)	f
1966	swbfront3wii	\N	Star Wars: Battlefront 3 (Wii)	f
1967	guitarh3xpwii	\N	Guitar Hero 3 Expansion Pack (Wii)	f
1968	callofduty4	\N	Call of Duty 4: Modern Warfare	f
1969	mmadnessexps3	\N	Monster Madness EX (PS3)	f
1970	mmadnessexps3am	\N	Monster Madness EX  Automatch (PS3)	f
1971	mmadexps3d	\N	Monster Madness EX Demo (PS3)	f
1972	terrortkdwn2	\N	Terrorist Takedown 2	f
1973	terrortkdwn2am	\N	Terrorist Takedown 2 Automatch	f
1974	terrortkdwn2d	\N	Terrorist Takedown 2 Demo	f
1975	cc3xp1mb	\N	Command & Conquer 3: Kane's Wrath Match Broadcast Clone	f
1976	dstallionds	\N	Derby Stallion DS (DS)	f
1977	blkuzushiwii	\N	THE Block Kuzushi - With the Stage Creation feature (Wii)	f
1978	thecombatwii	\N	SIMPLE Wii Series Vol.6 THE Minna de Waiwai Combat (Wii)	f
1979	cc3kw	\N	Command and Conquer 3 Kanes Wrath	f
1980	ballers3ps3	\N	NBA Ballers: Chosen One (PS3)	f
1981	ballers3ps3am	\N	NBA Ballers: Chosen One  Automatch (PS3)	f
1982	ballers3ps3d	\N	NBA Ballers: Chosen One  Demo (PS3)	f
1983	cc3kwmb	\N	Command and Conquer 3 Kanes Wrath Match Broadcast	f
1984	scourgepc	\N	The Scourge Project (PC)	f
1985	scourgepcam	\N	The Scourge Project  Automatch (PC)	f
1986	scourgepcd	\N	The Scourge Project  Demo (PC)	f
1987	scourgeps3	\N	The Scourge Project (PS3)	f
1988	scourgeps3am	\N	The Scourge Project  Automatch (PS3)	f
1989	scourgeps3d	\N	The Scourge Project  Demo (PS3)	f
1990	rfactoryEUds	\N	Rune Factory: A Fantasy Harverst Moon (EU) (DS)	f
1991	popwii	\N	Pop (WiiWare)	f
1992	tenchu4wii	\N	Tenchu 4 (Wii)	f
1993	ssoldierrwii	\N	Star Soldier R (WiiWare)	f
1994	2kboxingds	\N	2K Boxing (DS)	f
1995	bldragonds	\N	Blue Dragon (DS)	f
1996	elebitsds	\N	Elebits DS - Kai to Zero no Fushigi na Bus (DS)	f
1997	nobunagapktds	\N	Nobunaga no Yabou DS Pocket Senshi (DS)	f
1998	kqmateDS	\N	KaitoranmaQ Mate! (DS)	f
1999	digichampds	\N	Digimon Championship (DS)	f
2000	yakumanwii	\N	Yakuman Wii (WiiWare)	f
2001	mxvatvuPALps2	\N	MX vs ATV Untamed PAL (PS2)	f
2002	mxvatvuPALps2am	\N	MX vs ATV Untamed PAL  Automatch (PS2)	f
2003	mezasetm2wii	\N	Mezase!! Tsuri Master 2 (Wii)	f
2004	raw2009wii	\N	WWE Smackdown! vs RAW 2009 (Wii)	f
2005	redbaronps3	\N	Red Baron WWI (PS3)	f
2006	redbaronps3am	\N	Red Baron WWI  Automatch (PS3)	f
2007	memansf2USDS	\N	Mega Man Star Force 2: Zerker x Shinobi / Saurian (US)	f
2008	mxvatvutEUwii	\N	MX vs ATV Untamed (EU) (Wii)	f
2009	lostmagicwii	\N	Lost Magic Wii (Wii)	f
2010	shirends2ds	\N	Fushigi no Dungeon: Furai no Shiren DS2 (DS)	f
2011	worldshiftpcb	\N	WorldShift Beta (PC)	f
2012	worldshiftpcbam	\N	WorldShift Beta  Automatch (PC)	f
2013	blitz08ps3	\N	Blitz: The League 08 (PS3)	f
2014	blitz08ps3am	\N	Blitz: The League 08  Automatch (PS3)	f
2015	blitz08ps3d	\N	Blitz: The League 08  Demo (PS3)	f
2016	sangotends	\N	Sangokushitaisen Ten (DS)	f
2017	lonposwii	\N	Lonpos (WiiWare)	f
2018	cvania08ds	\N	Castlevania 2008 (DS)	f
2019	nplusds	\N	N+ (DS)	f
2020	gauntletds	\N	Gauntlet (DS)	f
2021	finertiaps3	\N	Fatal Inertia (PS3)	f
2022	finertiaps3am	\N	Fatal Inertia  Automatch (PS3)	f
2023	topspin3usds	\N	Top Spin 3 (US) (DS)	f
2024	topspin3euds	\N	Top Spin 3 (EU) (DS)	f
2025	wiilinkwii	\N	Wii Link (Wii)	f
2026	simcitywii	\N	SimCity Creator (Wii)	f
2027	tpfolEUpc	\N	Turning Point: Fall of Liberty (EU) (PC)	f
2028	tpfolEUpcam	\N	Turning Point: Fall of Liberty  Automatch (EU) (PC)	f
2029	tpfolEUpcd	\N	Turning Point: Fall of Liberty  Demo (EU) (PC)	f
2030	tpfolEUps3	\N	Turning Point: Fall of Liberty (EU) (PS3)	f
2031	tpfolEUps3am	\N	Turning Point: Fall of Liberty  Automatch (EU) (PS3)	f
2032	parabellumpc	\N	Parabellum (PC)	f
2033	parabellumpcam	\N	Parabellum  Automatch (PC)	f
2034	parabellumpcd	\N	Parabellum  Demo (PC)	f
2035	parabellumps3	\N	Parabellum (PS3)	f
2036	parabellumps3am	\N	Parabellum  Automatch (PS3)	f
2037	monlabwii	\N	Monster Lab (Wii)	f
2038	necrovisionpc	\N	NecroVision (PC)	f
2039	necrovisionpcam	\N	NecroVision  Automatch (PC)	f
2040	necrovisionpcd	\N	NecroVision  Demo (PC)	f
2041	necrovisionpd	\N	NecroVision (PC) Demo	f
2042	necrovisionpdam	\N	NecroVision  Automatch (PC) Demo	f
2043	damnationpc	\N	DamNation (PC)	f
2044	damnationpcam	\N	DamNation  Automatch (PC)	f
2045	damnationpcd	\N	DamNation  Demo (PC)	f
2046	damnationps3	\N	DamNation (PS3)	f
2047	damnationps3am	\N	DamNation  Automatch (PS3)	f
2048	strongholdce	\N	Stronghold: Crusader Extreme	f
2049	parabellumpcdam	\N	Parabellum  Demo  Automatch (PC)	f
2050	madeinoreds	\N	Made in Ore (DS)	f
2051	guinnesswrds	\N	Guinness World Records: The Video Game (DS)	f
2052	guinnesswrwii	\N	Guinness World Records: The Video Game (Wii)	f
2053	mclub4ps3dev	\N	Midnight Club 4 Dev (PS3)	f
2054	mclub4ps3devam	\N	Midnight Club 4 Dev  Automatch (PS3)	f
2055	mclub4xboxdev	\N	Midnight Club 4 Dev (Xbox360)	f
2056	mclub4xboxdevam	\N	Midnight Club 4 Dev  Automatch (Xbox360)	f
2057	gta4pcdev	\N	Grand Theft Auto 4 Dev (PC)	f
2058	gta4pcdevam	\N	Grand Theft Auto 4 Dev  Automatch (PC)	f
2059	gta4ps3dev	\N	Grand Theft Auto 4 Dev (PS3)	f
2060	gta4ps3devam	\N	Grand Theft Auto 4 Dev  Automatch (PS3)	f
2061	gts4xdev	\N	Grand Theft Auto 4 Dev (Xbox 360)	f
2062	gts4xdevam	\N	Grand Theft Auto 4 Dev  Automatch (Xbox 360)	f
2063	monfarm2ds	\N	Monster Farm DS 2 (DS)	f
2064	plunderpc	\N	Age of Booty (PC)	f
2065	plunderpcam	\N	Plunder  Automatch (PC)	f
2066	plunderpcd	\N	Plunder Demo (PC)	f
2067	legendaryps3	\N	Legendary (PS3)	f
2068	legendaryps3am	\N	Legendary  Automatch (PS3)	f
2069	callofduty4d2d	\N	Call of Duty 4: Modern Warfare	f
2070	sekainodokods	\N	Sekai no Dokodemo Shaberu! DS Oryori Navi (DS)	f
2071	glracerwii	\N	GameLoft's Racer (WiiWare)	f
2072	beijing08pc	\N	Beijing 2008 (PC)	f
2073	beijing08pcam	\N	Beijing 2008  Automatch (PC)	f
2074	beijing08pcd	\N	Beijing 2008  Demo (PC)	f
2075	beijing08ps3	\N	Beijing 2008 (PS3)	f
2076	beijing08ps3am	\N	Beijing 2008  Automatch (PS3)	f
2077	beijing08ps3d	\N	Beijing 2008  Demo (PS3)	f
2078	hail2chimps3	\N	Hail to the Chimp (PS3)	f
2079	hail2chimps3am	\N	Hail to the Chimp  Automatch (PS3)	f
2080	wlclashpc	\N	War Leaders: Clash of Nations (PC)	f
2081	wlclashpcam	\N	War Leaders: Clash of Nations  Automatch (PC)	f
2082	wlclashpcd	\N	War Leaders: Clash of Nations  Demo (PC)	f
2083	bomberman20ds	\N	Bomberman 2.0 (DS)	f
2084	heistpc	\N	Heist (PC)	f
2085	heistpcam	\N	Heist  Automatch (PC)	f
2086	heistpcd	\N	Heist  Demo (PC)	f
2087	heistps3	\N	Heist (PS3)	f
2088	heistps3am	\N	Heist  Automatch (PS3)	f
2089	bstrikeotspc	\N	Battlestrike: Operation Thunderstorm (PC)	f
2090	bstrikeotspcam	\N	Battlestrike: Operation Thunderstorm  Automatch (PC)	f
2091	bstrikeotspcd	\N	Battlestrike: Operation Thunderstorm  Demo (PC)	f
2092	Decathletesds	\N	Decathletes (DS)	f
2093	mleatingwii	\N	Major League Eating: The Game (EU/US) (WiiWare)	f
2094	cc3kwcd	\N	Command and Conquer 3 Kanes Wrath CD Key Auth	f
2095	cc3kwcdam	\N	Command and Conquer 3 Kanes Wrath CD Key Auth Automatch	f
2096	gta4ps3grm	\N	Grand Theft Auto 4 German (PS3)	f
2097	gta4ps3grmam	\N	Grand Theft Auto 4 German  Automatch (PS3)	f
2098	gta4xgrm	\N	Grand Theft Auto 4 German (Xbox 360)	f
2099	gta4xgrmam	\N	Grand Theft Auto 4 German  Automatch (Xbox 360)	f
2100	cc3tibwarscd	\N	Command & Conquer 3: Tiberium Wars CD Key Auth	f
2101	cc3tibwarscdam	\N	Command & Conquer 3: Tiberium Wars CD Key Auth Automatch	f
2102	arkaUSEUds	\N	Arkanoid DS (US/EU) (DS)	f
2103	madden09ds	\N	Madden NFL 09 (DS)	f
2104	tetpartywii	\N	Tetris Party (WiiWare)	f
2105	frontlinesfow	\N	Frontlines: Fuel of War	f
2106	simspartywii	\N	MySims Party (Wii)	f
2107	momotaro20ds	\N	Momotaro Dentetsu 20 Shuunen (DS)	f
2108	srow2ps3	\N	Saint's Row 2 (PS3)	f
2109	srow2ps3am	\N	Saint's Row 2 Automatch	f
2110	srow2pc	\N	Saint's Row 2 (PC)	f
2111	srow2pcam	\N	Saint's Row 2  Automatch (PC)	f
2112	srow2xb360	\N	Saint's Row 2 (Xbox 360)	f
2113	srow2xb360am	\N	Saint's Row 2  Automatch (Xbox 360)	f
2116	aliencrashwii	\N	Alien Crash (WiiWare)	f
2117	nakedbrbndds	\N	Naked Brothers Band World of Music Tour (DS)	f
2118	legendarypc	\N	Legendary (PC)	f
2119	legendarypcam	\N	Legendary  Automatch (PC)	f
2120	legendarypcd	\N	Legendary Demo (PC)	f
2121	megaman9wii	\N	Mega Man 9 (WiiWare)	f
2122	dqmonjoker2ds	\N	Dragon Quest Monsters: Joker 2 (DS)	f
2123	quizmagicds	\N	Quiz Magic Academy DS (DS)	f
2124	Narutonin2ds	\N	Naruto: Path of the Ninja 2 (DS)	f
2125	draglade2ds	\N	Custom Beat Battle: Draglade 2 (DS)	f
2126	mfcoachwii	\N	My Fitness Coach (Wii)	f
2127	othellods	\N	Othello de Othello DS	f
2128	redalert3pc	\N	Red Alert 3 (PC)	f
2129	redalert3pcam	\N	Red Alert 3  Automatch (PC)	f
2130	redalert3pcd	\N	Red Alert 3  Demo (PC)	f
2131	redalert3ps3	\N	Red Alert 3 (PS3)	f
2132	redalert3ps3am	\N	Red Alert 3  Automatch (PS3)	f
2133	plunderps3	\N	Age of Booty (PS3)	f
2134	plunderps3am	\N	Plunder  Automatch (PS3)	f
2135	plunderps3d	\N	Plunder  Demo (PS3)	f
2136	svsr09ps3	\N	WWE Smackdown vs. RAW 2009 (PS3)	f
2137	wordjongFRds	\N	Word Jong - FR (DS)	f
2138	bbangminids	\N	Big Bang Mini (DS)	f
2139	swbf3psp	\N	Star Wars: Battlefront 3 (PSP)	f
2140	swbf3pspam	\N	Star Wars: Battlefront 3  Automatch (PSP)	f
2141	CMwrldkitwii	\N	Cooking Mama: World Kitchen (Wii)	f
2142	shootantowii	\N	Shootanto (Wii)	f
2143	punchoutwii	\N	Punch-Out!! (Wii)	f
2144	cod5victoryds	\N	Call of Duty 5: Victory (DS)	f
2145	ultibandwii	\N	Ultimate Band (Wii)	f
2146	CVjudgmentwii	\N	Castlevania: Judgment (Wii)	f
2147	ageofconanb	\N	Age of Conan beta	f
2148	wrldgoowii	\N	World of Goo (WiiWare)	f
2149	saadtestam	\N	SaadsTest	f
2151	digichampUSds	\N	Digimon Championship (US) (DS)	f
2152	scotttestam	\N	Scott's test gamename Automatch	f
2153	testam	\N	Test Automatch	f
2154	tpfolEUpcB	\N	Turning Point: Fall of Liberty (EU-B) (PC)	f
2155	tpfolEUpcBam	\N	Turning Point: Fall of Liberty  Automatch (EU-B) (PC)	f
2156	tpfolEUpcBd	\N	Turning Point: Fall of Liberty  Demo (EU-B) (PC)	f
2157	tpfolpcB	\N	Turning Point: Fall of Liberty (B) (PC)	f
2158	tpfolpcBam	\N	Turning Point: Fall of Liberty  Automatch (B) (PC)	f
2159	tpfolpcBd	\N	Turning Point: Fall of Liberty  Demo (B) (PC)	f
2160	cc3arenapc	\N	Command & Conquer: Arena	f
2161	cc3arenapcam	\N	Command & Conquer: Arena Automatch	f
2162	cc3arenapcd	\N	Command & Conquer: Arena Demo	f
2163	swbfront3ps3	\N	Star Wars Battlefront 3 (PS3)	f
2164	swbfront3pcCam	\N	Star Wars Battlefront 3  Automatch (PS3)	f
2165	coh2pc	\N	Code of Honor 2 (PC)	f
2166	coh2pcam	\N	Code of Honor 2  Automatch (PC)	f
2167	dimensitypc	\N	Dimensity (PC)	f
2168	dimensitypcam	\N	Dimensity  Automatch (PC)	f
2169	dimensitypcd	\N	Dimensity  Demo (PC)	f
2170	gta4ps3test	\N	Grand Theft Auto 4 Test (PS3)	f
2171	50centsandps3	\N	50 Cent: Blood on the Sand (PS3)	f
2172	50centsandps3am	\N	50 Cent: Blood on the Sand  Automatch (PS3)	f
2173	locksquestds	\N	Construction Combat: Lock's Quest	f
2174	srow2ps3d	\N	Saint's Row 2 (PS3) Demo	f
2175	srow2ps3dam	\N	Saint's Row 2  Automatch (PS3) Demo	f
2176	nobuyabou2ds	\N	Nobunaga no Yabou DS 2 (DS)	f
2177	memansf2EUDS	\N	Mega Man Star Force 2: Zerker x Shinobi / Saurian (EU)	f
2178	bleach2EUds	\N	Bleach DS 2: Requiem in the black robe (EU) (DS)	f
2179	reichpc	\N	Reich (PC)	f
2180	reichpcam	\N	Reich  Automatch (PC)	f
2181	reichps3	\N	Reich (PS3)	f
2182	reichps3am	\N	Reich  Automatch (PS3) Clone	f
2183	gloftpokerwii	\N	Gameloft Poker (WiiWare)	f
2184	saspc	\N	SAS (PC)	f
2185	saspcam	\N	SAS  Automatch (PC)	f
2186	toribashwii	\N	Toribash (WiiWare)	f
2187	acrossingwii	\N	Animal Crossing Wii (Wii)	f
2188	poriginps3	\N	Fear 2: Project Origin (PS3)	f
2189	poriginps3am	\N	Fear 2: Project Origin  Automatch (PS3)	f
2190	poriginps3d	\N	Fear 2: Project Origin  Demo (PS3)	f
2191	poriginpc	\N	Fear 2: Project Origin (PC)	f
2192	poriginpcam	\N	Fear 2: Project Origin  Automatch (PC)	f
2193	poriginpcd	\N	Fear 2: Project Origin  Demo (PC)	f
2194	civ4xp3	lgNJU7	Civilization IV: 3rd Expansion	f
2195	civ4xp3am	lgNJU7	Civilization IV: 3rd Expansion Automatch	f
2196	civ4xp3d	lgNJU7	Civilization IV: 3rd Expansion Demo	f
2197	necrovision	\N	NecroVision	f
2198	tecmoblkickds	\N	Tecmo Bowl Kickoff (DS)	f
2199	bballarenaps3	\N	Supersonic Acrobatic Rocket-Powered BattleCars: BattleBall Arena	f
2200	bballarenaps3am	\N	Supersonic Acrobatic Rocket-Powered BattleCars: BattleBall Arena Automatch	f
2201	ragunonlineds	\N	Ragunaroku Online DS (DS)	f
2202	mlb2k9ds	\N	Major League Baseball 2K9 Fantasy All-Stars (DS)	f
2203	kororinpa2wii	\N	Kororinpa 2 (Wii)	f
2204	stlprincessds	\N	Steal Princess (DS)	f
2205	aoemythds	\N	Age of Empires: Mythologies (DS)	f
2206	raymanRR3wii	\N	Rayman Raving Rabbids 3 (Wii)	f
2207	pitcrewwii	\N	Pit Crew Panic (WiiWare)	f
2208	crysiswars	zKbZiM	Crysis Wars	f
2209	menofwarpc	\N	Men of War (PC)	f
2210	menofwarpcam	\N	Men of War  Automatch (PC)	f
2211	sakatsukuds	\N	Sakatsuku DS (DS)	f
2212	wtrwarfarewii	\N	Water Warfare (WiiWare)	f
2213	civ4colpc	2yheDS	Sid Meier's Civilization IV: Colonization (PC/Mac)	f
2214	civ4colpcam	2yheDS	Sid Meier's Civilization IV: Colonization  Automatch (PC)	f
2215	civ4colpcd	2yheDS	Sid Meier's Civilization IV: Colonization  Demo (PC)	f
2216	tablegamestds	\N	Table Game Stadium (D3-Yuki) (Wii)	f
2217	ppkpocket11ds	\N	PowerPro-kun Pocket 11 (DS)	f
2218	bleach2wii	\N	BLEACH Wii2 (Wii)	f
2219	cuesportswii	\N	Cue Sports (WiiWare)	f
2220	svsr09x360	\N	WWE Smackdown vs. RAW 2009 (Xbox 360)	f
2221	hail2chimps3d	\N	Hail to the Chimp  Demo (PS3)	f
2222	cod5wii	\N	Call of Duty 5 (Wii)	f
2223	bgeverwii	\N	Best Game Ever (WiiWare)	f
2224	spinvgewii	\N	Space Invaders: Get Even (WiiWare)	f
2225	tstgme	\N	test game	f
2226	TerroristT2	\N	Terrorist Takedown 2	f
2227	pokemonplatds	\N	Pokemon Platinum (DS)	f
2228	redalert3pcmb	\N	Red Alert 3 (PC) Match Broadcast	f
2229	redalert3pcdmb	\N	Red Alert 3 Demo (PC) Match Broadcast	f
2230	jbond08wii	\N	James Bond 2008 (Wii)	f
2231	assultwii	\N	Assult (Wii)	f
2232	mmartracewii	\N	Mega Mart Race (WiiWare)	f
2233	fifa09ds	\N	FIFA 09 Soccer (DS)	f
2234	bboarderswii	\N	Battle Boarders (WiiWare)	f
2235	cellfactorpsn	\N	CellFactor: Ignition (PSN)	f
2236	cellfactorpsnam	\N	CellFactor: Ignition  Automatch (PSN)	f
2237	witcher	\N	The Witcher	f
2238	redalert3pcb	\N	Red Alert 3 Beta (PC)	f
2239	redalert3pcbam	\N	Red Alert 3  Beta (PC) Automatch	f
2241	redalert3pcbmb	\N	Red Alert 3 Beta (PC) Match Broadcast	f
2242	redalert3pcdam	\N	Red Alert 3  Demo Automatch (PC)	f
2243	opblitz	\N	Operation Blitzsturm	f
2244	shogiwii	\N	Shogi (Wii) (WiiWare)	f
2245	redfactionwii	\N	Red Faction Wii (Wii)	f
2246	ryantest	\N	Ryan'st test gamename	f
2247	ryantestam	\N	Ryan'st test gamename Automatch	f
2248	mkvsdcps3	\N	Mortal Kombat vs. DC Universe (PS3)	f
2249	mkvsdcps3am	\N	Mortal Kombat vs. DC Universe  Automatch (PS3)	f
2250	mkvsdcxbox	\N	Mortal Kombat vs. DC Universe (Xbox)	f
2251	mkvsdcxboxam	\N	Mortal Kombat vs. DC Universe  Automatch (Xbox)	f
2252	blzrdriverds	\N	Blazer Drive (DS)	f
2253	micchannelwii	\N	Mic Chat Channel (Wii)	f
2254	rman2blkredds	\N	Ryusei no Rockman 3: Black Ace / Red Joker (JP) (DS)	f
2255	jnglspeedwii	\N	Jungle Speed (WiiWare)	f
2259	kingtigerspc	\N	King Tigers (PC)	f
2260	kingtigerspcam	\N	King Tigers  Automatch (PC)	f
2261	kingtigerspcd	\N	King Tigers  Demo (PC)	f
2262	hail2chimps3r	\N	Hail to the Chimp Retail (PS3)	f
2263	hail2chimps3ram	\N	Hail to the Chimp Retail  Automatch (PS3)	f
2264	stalkercs	\N	Stalker: Clear Sky (PC)	f
2265	stalkercsam	\N	Stalker: Clear Sky  Automatch (PC)	f
2266	stalkercsd	\N	Stalker: Clear Sky  Demo (PC)	f
2267	gradiusrbwii	\N	Gradius ReBirth (WiiWare)	f
2268	radiohitzwii	\N	Radiohitz: Guess That Song! (WiiWare)	f
2269	kkhrebornwii	\N	Katei Kyoshi Hitman REBORN! Kindan no Yami no Delta (Wii)	f
2270	fstarzerods	\N	Fantasy Star ZERO (DS)	f
2271	igowii	\N	Igo (Wii) (WiiWare)	f
2272	bokujyomonds	\N	Bokujyo Monogatari Youkoso! Kaze no Bazzare (DS)	f
2273	hotncoldds	\N	Hot 'n' Cold (DS)	f
2274	ut3jpps3	\N	Unreal Tournament 3 Japanese (PS3)	f
2275	ut3jpps3am	\N	Unreal Tournament 3 Japanese  Automatch (PS3)	f
2276	ut3jppc	\N	Unreal Tournament 3 Japanese (PC)	f
2277	ut3jppcam	\N	Unreal Tournament 3 Japanese  Automatch (PC)	f
2278	AliensCMPC	\N	Aliens: Colonial Marines (PC)	f
2279	AliensCMPCam	\N	Aliens: Colonial Marines  Automatch (PC)	f
2280	AliensCMPCd	\N	Aliens: Colonial Marines  Demo (PC)	f
2281	AliensCMPS3	\N	Aliens: Colonial Marines (PS3)	f
2282	AliensCMPS3am	\N	Aliens: Colonial Marines  Automatch (PS3)	f
2283	AliensCMPS3d	\N	Aliens: Colonial Marines  Demo (PS3)	f
2284	Majesty2PC	\N	Majesty 2 (PC)	f
2285	Majesty2PCam	\N	Majesty 2  Automatch (PC)	f
2286	Majesty2PCd	\N	Majesty 2  Demo (PC)	f
2287	FlockPC	\N	Flock (PC)	f
2288	FlockPCam	\N	Flock  Automatch (PC)	f
2289	FlockPCd	\N	Flock  Demo (PC)	f
2290	FlockPSN	\N	Flock (PSN)	f
2293	FlockPSNd	\N	Flock  Demo (PSN)	f
2294	FlockPSNam	\N	Flock  Automatch (PSN)	f
2295	bbobblewii	\N	Bubble Bobble Wii (WiiWare)	f
2296	cellfactorpc	\N	CellFactor: Ignition (PC)	f
2297	cellfactorpcam	\N	CellFactor: Ignition  Automatch (PSN) Clone	f
2298	wormspsp	\N	Worms (PSP)	f
2299	wormspspam	\N	Worms  Automatch (PSP)	f
2300	MotoGP08PC	\N	MotoGP08 (PC)	f
2301	MotoGP08PCam	\N	MotoGP08  Automatch (PC)	f
2302	MotoGP08PS3	\N	MotoGP08 (PS3)	f
2303	MotoGP08PS3am	\N	MotoGP08  Automatch (PS3)	f
2304	sonicbkwii	\N	Sonic and the Black Knight (Wii)	f
2305	ghero4wii	\N	Guitar Hero 4 (Wii)	f
2306	digichampKRds	\N	Digimon Championship (KOR) (DS)	f
2307	mswinterwii	\N	Mario & Sonic at the Olympic Winter Games (Wii)	f
2309	mswinterds	\N	Mario & Sonic at the Olympic Winter Games (DS)	f
2310	fmasterwtwii	\N	Fishing Master: World Tour (Wii)	f
2311	starpballwii	\N	Starship Pinball (WiiWare)	f
2312	nfsmwucoverds	\N	Need for Speed Most Wanted: Undercover (DS)	f
2313	neopetspapc	\N	Neopets Puzzle Adventure (PC)	f
2314	neopetspapcam	\N	Neopets Puzzle Adventure  Automatch (PC)	f
2315	neopetspapcd	\N	Neopets Puzzle Adventure  Demo (PC)	f
2316	luminarc2USds	\N	Luminous Arc 2 Will (US) (DS)	f
2317	chocotokids	\N	Shido to Chocobo no Fushigina Dungeon Tokiwasure no Meikyu DS+(DS)	f
2318	monkmayhemwii	\N	Maniac Monkey Mayhem (WiiWare)	f
2319	takoronKRwii	\N	Takoron (KOR) (Wii)	f
2321	kaosmpr	\N	Kaos MPR	f
2322	kaosmpram	\N	Kaos MPR Automatch	f
2323	kaosmprd	\N	Kaos MPR Demo	f
2420	sawps3	\N	SAW (PS3)	f
2324	mcdcrewds	\N	McDonald's DS Crew Development Program (DS)	f
2325	ufc09ps3	\N	UFC 2009 (PS3)	f
2326	ufc09ps3am	\N	UFC 2009 Automatch (PS3)	f
2327	ufc09ps3d	\N	UFC 2009 Demo (PS3)	f
2328	ufc09x360	\N	UFC 2009 (Xbox 360)	f
2329	ufc09x360am	\N	UFC 2009 Automatch (Xbox 360)	f
2330	ufc09x360d	\N	UFC 2009 Demo (Xbox 360)	f
2331	skateitds	\N	Skate It (DS)	f
2332	robolypsewii	\N	Robocalypse (WiiWare)	f
2333	puffinsds	\N	Puffins: Island Adventures (DS)	f
2334	koinudewii	\N	Koinu de Kururin Wii (WiiWare)	f
2335	lonposUSwii	\N	Lonpos (US) (WiiWare)	f
2336	wwkuzushiwii	\N	SIMPLE THE Block Kuzushi (WiiWare)	f
2337	wwpuzzlewii	\N	Simple: The Number - Puzzle	f
2338	snightxds	\N	Summon Night X (DS)	f
2339	hotrodwii	\N	High Voltage Hod Rod Show (WiiWare)	f
2340	hotrodwiiam	\N	High Voltage Hod Rod Show  Automatch (WiiWare)	f
2341	spbobbleds	\N	Space Puzzle Bobble (DS)	f
2342	bbarenaEUps3	\N	Supersonic Acrobatic Rocket-Powered BattleCars (PSN) (EU)	f
2343	bbarenaEUps3am	\N	Supersonic Acrobatic Rocket-Powered BattleCars  Automatch (PSN) (EU)	f
2344	bbarenaJPNps3	\N	Supersonic Acrobatic Rocket-Powered BattleCars (PSN) (JPN)	f
2345	bbarenaJPNps3am	\N	Supersonic Acrobatic Rocket-Powered BattleCars  Automatch (PSN) (JPN)	f
2346	girlssecEUds	\N	Winx Club Secret Diary 2009 (EU) (DS)	f
2347	ffccechods	\N	Final Fantasy Crystal Chronicles: Echos of Time (Wii/DS)	f
2348	unbballswii	\N	Unbelievaballs (Wii)	f
2349	hokutokenwii	\N	Hokuto no Ken (WiiWare)	f
2350	monracersds	\N	Monster Racers (DS)	f
2351	tokyoparkwii	\N	Tokyo Friend Park II Wii (Wii)	f
2352	derbydogwii	\N	Derby Dog (WiiWare)	f
2353	bbarenaEUps3d	\N	Supersonic Acrobatic Rocket-Powered BattleCars  Demo (PSN) (EU)	f
2354	bbarenaJPps3d	\N	Supersonic Acrobatic Rocket-Powered BattleCars  Demo (PSN) (JPN)	f
2355	ballarenaps3d	\N	Supersonic Acrobatic Rocket-Powered BattleCars: BattleBall Arena Demo	f
2356	fuelpc	\N	FUEL (PC)	f
2357	fuelpcam	\N	FUEL Automatch (PC)	f
2358	fuelpcd	\N	FUEL Demo (PC)	f
2359	fuelps3	\N	FUEL (PS3)	f
2360	fuelps3am	\N	FUEL Automatch (PS3)	f
2361	fuelps3d	\N	FUEL Demo (PS3)	f
2362	mleatingJPwii	\N	Major League Eating: The Game (JPN) (WiiWare)	f
2363	mleatingJPwiiam	\N	Major League Eating: The Game  Automatch (JPN) (WiiWare)	f
2364	octoEUwii	\N	Octomania (EU) (Wii)	f
2365	sbkUSps3	\N	SBK '08 (US) (PS3)	f
2366	sbkUSps3am	\N	SBK '08  Automatch (US) (PS3)	f
2367	sbkUSps3d	\N	SBK '08  Demo (US) (PS3)	f
2368	sbkUSpc	\N	SBK '08 (US) (PC)	f
2369	sbkUSpcam	\N	SBK '08  Automatch (US) (PC)	f
2370	sbkUSpcd	\N	SBK '08  Demo (US) (PC)	f
2371	medarotds	\N	MedaRot DS (DS)	f
2372	idolmasterds	\N	The Idolmaster DS (DS)	f
2373	ekorisu2ds	\N	Ekorisu 2 (DS)	f
2374	redalert3pccd	\N	Red Alert 3 (PC, CDKey)	f
2375	redalert3pccdam	\N	Red Alert 3  Automatch (PC, CDKey)	f
2376	bbladeds	\N	Bay Blade (DS)	f
2377	takameijinwii	\N	Takahashi Meijin no Boukenshima (WiiWare)	f
2379	texasholdwii	\N	Texas Hold'em Tournament (WiiWare)	f
2380	legofwreps3	\N	WWE Legends of Wrestlemania (PS3)	f
2381	legofwreps3am	\N	WWE Legends of Wrestlemania  Automatch (PS3)	f
2382	legofwrex360	\N	WWE Legends of Wrestlemania (Xbox 360)	f
2383	legofwrex360am	\N	Legends of Wrestlemania  Automatch (Xbox 360)	f
2384	warnbriads	\N	Warnbria no Maho Kagaku (DS)	f
2385	tsurimasterds	\N	Mezase!! Tsuri Master DS (DS)	f
2386	jikkyonextwii	\N	Jikkyo Powerful Pro Yakyu NEXT (Wii)	f
2387	mua2wii	\N	Marvel Ultimate Alliance 2: Fusion (Wii)	f
2388	civrevasiaps3	\N	Civilization Revolution (Asia) (PS3)	f
2391	pocketwrldds	\N	My Pocket World (DS)	f
2392	segaracingds	\N	Sega Superstars Racing (DS)	f
2393	segaracingwii	\N	Sega Superstars Racing (Wii)	f
2394	3dpicrossds	\N	3D Picross (DS)	f
2395	mkvsdcEUps3	\N	Mortal Kombat vs. DC Universe (EU) (PS3)	f
2396	mkvsdcEUps3am	\N	Mortal Kombat vs. DC Universe  Automatch (EU) (PS3)	f
2397	mkvsdcEUps3b	\N	Mortal Kombat vs. DC Universe  Beta (EU) (PS3)	f
2398	mkvsdcps3b	\N	Mortal Kombat vs. DC Universe Beta (PS3)	f
2399	mkvsdcps3bam	\N	Mortal Kombat vs. DC Universe Beta  Automatch (PS3)	f
2400	liightwii	\N	Liight (WiiWare)	f
2401	mogumonwii	\N	Tataite! Mogumon (WiiWare)	f
2402	weleplay09wii	\N	Winning Eleven PLAY MAKER 2009 (Wii)	f
2403	mini4wdds	\N	Mini 4WD DS (DS)	f
2404	puzzshangwii	\N	Puzzle Games Shanghai Wii (WiiWare)	f
2405	crystalw1wii	\N	Crystal - Defender W1 (WiiWare)	f
2406	crystalw2wii	\N	Crystal - Defender W2 (WiiWare)	f
2407	overturnwii	\N	Overturn (WiiWare)	f
2408	vtennisacewii	\N	Virtua Tennis: Ace (Wii)	f
2409	yugioh5dds	\N	Yu-Gi-Oh 5Ds (DS)	f
2410	im1pc	\N	Interstellar Marines (PC)	f
2411	im1pcam	\N	Interstellar Marines  Automatch (PC)	f
2412	im1pcd	\N	Interstellar Marines Demo (PC)	f
2413	civrevasips3d	\N	Civilization Revolution Demo (Asia) (PS3)	f
2414	civrevoasiads	\N	Sid Meier's Civilization Revolution (DS, Asia)	f
2415	50ctsndlvps3	\N	50 Cent: Blood on the Sand - Low Violence (PS3)	f
2416	50ctsndlvps3am	\N	50 Cent: Blood on the Sand - Low Violence  Automatch (PS3)	f
2417	sawpc	\N	SAW (PC)	f
2418	sawpcam	\N	SAW  Automatch (PC)	f
2421	sawps3am	\N	SAW  Automatch (PS3)	f
2422	sawps3d	\N	SAW  Demo (PS3)	f
2423	ssmahjongwii	\N	Simple Series: The Mah-Jong (WiiWare)	f
2424	carnivalkwii	\N	Carnival King (WiiWare)	f
2425	pubdartswii	\N	Pub Darts (WiiWare)	f
2426	biahhJPps3	\N	Brothers In Arms: Hell's Highway (PS3) (JPN)	f
2427	biahhJPps3am	\N	Brothers In Arms: Hell's Highway  Automatch (PS3) (JPN)	f
2428	biahhJPps3d	\N	Brothers In Arms: Hell's Highway  Demo (PS3) (JPN)	f
2429	codwawbeta	\N	Call of Duty: World at War Beta	f
2430	fallout3	\N	Fallout 3	f
2431	taprace	\N	Tap Race (iPhone Sample)	f
2432	tapraceam	\N	Tap Race Automatch (iPhone Sample)	f
2433	callofduty5	\N	Call of Duty 5	f
2434	cnpanzers2cwb	\N	Codename Panzers 2: Cold Wars BETA (PC)	f
2435	cnpanzers2cwbam	\N	Codename Panzers 2: Cold Wars BETA  Automatch (PC)	f
2436	biahhPRps3	\N	Brothers In Arms: Hell's Highway (PS3) (RUS)	f
2437	biahhPRps3am	\N	Brothers In Arms: Hell's Highway  Automatch (PS3) (POL/RUS)	f
2438	biahhPRps3d	\N	Brothers In Arms: Hell's Highway  Demo (PS3) (RUS)	f
2643	pokedngnwii	\N	Pokemon Dungeon (Wii)	f
2439	biahhPCHpc	\N	Brothers In Arms: Hell's Highway (PC) (POL/CZE/HUNG)	f
2440	biahhPCHpcam	\N	Brothers In Arms: Hell's Highway  Automatch (PC) (POL/CZE/HUNG)	f
2441	biahhRUSpc	\N	Brothers In Arms: Hell's Highway (PC) (RUS)	f
2442	biahhRUSpcam	\N	Brothers In Arms: Hell's Highway  Automatch (PC) (RUS)	f
2443	stormrisepc	\N	Stormrise (PC)	f
2444	stormrisepcam	\N	Stormrise Automatch (PC)	f
2445	stormrisepcd	\N	Stormrise Demo (PC)	f
2446	stlprinKORds	\N	Steal Princess (KOR) (DS)	f
2447	kaiwanowads	\N	KAIWANOWA (DS)	f
2448	mvsdk25ds	\N	Mario vs Donkey Kong 2.5 (DS)	f
2449	stlprinEUds	\N	Steal Princess (EU) (DS)	f
2450	gh4metalwii	\N	Guitar Hero 4: Metallica (Wii)	f
2451	psyintdevpc	\N	Psyonix Internal Development (PC)	f
2452	psyintdevpcam	\N	Psyonix Internal Development  Automatch (PC)	f
2453	psyintdevpcd	\N	Psyonix Internal Development  Demo (PC)	f
2454	simsracingds	\N	MySims Racing DS (DS)	f
2455	airhockeywii	\N	World Air Hockey Challenge! (WiiWare)	f
2456	evaspacewii	\N	Evasive Space (WiiWare)	f
2457	spaceremixds	\N	Space Invaders Extreme Remix (DS)	f
2458	menofwarpcb	\N	Men of War (PC) BETA	f
2459	menofwarpcbam	\N	Men of War  Automatch (PC) BETA	f
2460	codwaw	\N	Call of Duty: World at War	f
2461	kentomashods	\N	Ide Yohei no Kento Masho DS (DS)	f
2462	beatrunnerwii	\N	Beat Runner (WiiWare)	f
2463	hunterdanwii	\N	Hunter Dan's Triple Crown Tournament Fishing (Wii)	f
2464	rainbowislwii	\N	Rainbow Island Tower! (WiiWare)	f
2465	srgakuends	\N	Super Robot Gakuen (DS)	f
2466	srgakuendsam	\N	Super Robot Gakuen  Automatch (DS)	f
2467	cstaisends	\N	Chotto Sujin Taisen (DS)	f
2468	winx2010ds	\N	Winx Club Secret Diary 2010 (DS)	f
2469	mxravenpsp	\N	MX Reflex (Raven) (PSP)	f
2470	mxravenpspam	\N	MX Raven  Automatch (PSP)	f
2471	sukashikds	\N	Sukashikashipanman DS (DS)	f
2472	famista09ds	\N	Pro Yakyu Famista DS 2009 (DS)	f
2473	hawxpc	\N	Tom Clancy's HAWX	f
2474	fxtrainingds	\N	FX Training DS (DS)	f
2475	monhuntergwii	\N	Monster Hunter G (Wii)	f
2476	dinerdashwii	\N	Diner Dash (WiiWare)	f
2477	s_l4d	\N	Steam Left 4 Dead	f
2478	guinnesswriph	\N	Guinness World Records: The Video Game (iPhone)	f
2479	guinnesswripham	\N	Guinness World Records: The Video Game  Automatch (iPhone)	f
2480	guinnesswriphd	\N	Guinness World Records: The Video Game  Demo (iPhone)	f
2481	konsportswii	\N	Konami Sports Club @ Home (WiiWare)	f
2482	cpenguin2ds	\N	Club Penguin 2 (DS)	f
2483	biahhPOLps3	\N	Brothers In Arms: Hell's Highway (PS3) (POL)	f
2484	biahhPOLps3am	\N	Brothers In Arms: Hell's Highway  Automatch (PS3) (POL)	f
2485	biahhPOLps3d	\N	Brothers In Arms: Hell's Highway  Demo (PS3) (POL)	f
2486	exciteracewii	\N	Excite Racing (Wii)	f
2487	cpenguin2wii	\N	Club Penguin 2 (Wii)	f
2488	tcounterwii	\N	Tecmo Counter	f
2489	h2cdigitalps3	\N	Hail to the Chimp (PSN)	f
2490	h2cdigitalps3d	\N	Hail to the Chimp  Demo (PSN)	f
2491	motogp09ps3	\N	Moto GP 09 (PS3)	f
2492	motogp09ps3am	\N	Moto GP 09  Automatch (PS3)	f
2493	motogp09ps3d	\N	Moto GP 09  Demo (PS3)	f
2494	motogp09pc	\N	Moto GP 09 (PC)	f
2495	motogp09pcam	\N	Moto GP 09  Automatch (PC)	f
2496	motogp09pcd	\N	Moto GP 09  Demo (PC)	f
2497	spectro2wii	\N	Spectrobes 2 (Wii)	f
2498	ninTest/	\N	Nintendo Development Testing masterID 0	f
2499	ninTest/am	\N	Nintendo Development Testing masterID 0 Automatch	f
2500	ninTest0	\N	Nintendo Development Testing masterID 1	f
2501	ninTest0am	\N	Nintendo Development Testing masterID 1 Automatch	f
2502	ninTest1	\N	Nintendo Development Testing masterID 2	f
2503	ninTest1am	\N	Nintendo Development Testing masterID 2 Automatch	f
2504	ninTest2	\N	Nintendo Development Testing masterID 3	f
2505	ninTest2am	\N	Nintendo Development Testing masterID 3 Automatch	f
2506	ninTest3	\N	Nintendo Development Testing masterID 4	f
2507	ninTest3am	\N	Nintendo Development Testing masterID 4 Automatch	f
2508	ninTest4	\N	Nintendo Development Testing masterID 5	f
2509	ninTest4am	\N	Nintendo Development Testing masterID 5 Automatch	f
2510	ninTest5	\N	Nintendo Development Testing masterID 6	f
2603	brigades	\N	Gamespy Brigades	f
2511	ninTest5am	\N	Nintendo Development Testing masterID 6 Automatch	f
2512	ninTest6	\N	Nintendo Development Testing masterID 7	f
2513	ninTest6am	\N	Nintendo Development Testing masterID 7 Automatch	f
2514	ninTest7	\N	Nintendo Development Testing masterID 8	f
2515	ninTest7am	\N	Nintendo Development Testing masterID 8 Automatch	f
2516	ninTest8	\N	Nintendo Development Testing masterID 9	f
2517	ninTest8am	\N	Nintendo Development Testing masterID 9 Automatch	f
2518	ninTest9	\N	Nintendo Development Testing masterID 10	f
2519	ninTest9am	\N	Nintendo Development Testing masterID 10 Automatch	f
2521	ninTest:am	\N	Nintendo Development Testing masterID 11 Automatch	f
2522	ninTest;	\N	Nintendo Development Testing masterID 12	f
2523	ninTest;am	\N	Nintendo Development Testing masterID 12 Automatch	f
2524	ninTest<	\N	Nintendo Development Testing masterID 13	f
2525	ninTest<am	\N	Nintendo Development Testing masterID 13 Automatch	f
2526	ninTest=	\N	Nintendo Development Testing masterID 14	f
2527	ninTest=am	\N	Nintendo Development Testing masterID 14 Automatch	f
2528	ninTest>	\N	Nintendo Development Testing masterID 15	f
2529	ninTest>am	\N	Nintendo Development Testing masterID 15 Automatch	f
2530	ninTest?	\N	Nintendo Development Testing masterID 16	f
2531	ninTest?am	\N	Nintendo Development Testing masterID 16 Automatch	f
2532	ninTest@	\N	Nintendo Development Testing masterID 17	f
2533	ninTest@am	\N	Nintendo Development Testing masterID 17 Automatch	f
2534	ninTest-	\N	Nintendo Development Testing masterID 18	f
2535	ninTest-am	\N	Nintendo Development Testing masterID 18 Automatch	f
2536	ninTest.	\N	Nintendo Development Testing masterID 19	f
2537	ninTest.am	\N	Nintendo Development Testing masterID 19 Automatch	f
2538	dartspartywii	\N	Darts Wii Party (Wii)	f
2539	3celsiuswii	\N	3* Celsius (WiiWare)	f
2540	acejokerUSds	\N	Mega Man Star Force 3: Black Ace/Red Joker (US) (DS)	f
2541	Rabgohomewii	\N	Rabbids Go Home (Wii)	f
2542	tmntsmashwii	\N	TMNT Smash Up (Wii)	f
2543	simplejudowii	\N	Simple The Ju-Do (WiiWare)	f
2544	menofwarpcd	\N	Men of War MP DEMO (PC)	f
2545	menofwarpcdam	\N	Men of War MP DEMO  Automatch (PC)	f
2547	rdr2ps3	\N	Red Dead Redemption (PS3)	f
2548	rdr2ps3am	\N	Red Dead Redemption  Automatch (PS3)	f
2549	gh4vhalenwii	\N	Guitar Hero 4: Van Halen (Wii)	f
2550	gh4vhalenwiiam	\N	Guitar Hero 4: Van Halen  Automatch (Wii)	f
2551	escviruswii	\N	Escape Virus (WiiWare)	f
2552	rfactoryKRds	\N	Rune Factory: A Fantasy Harverst Moon (KOR) (DS)	f
2553	banburadxds	\N	Banbura DX Photo Frame Radio (DS)	f
2554	mebiuswii	\N	Mebius Drive (WiiWare)	f
2555	okirakuwii	\N	Okiraku Daihugou Wii (WiiWare)	f
2556	sbk09pc	\N	SBK '09 (PC)	f
2557	sbk09ps3	\N	SBK '09 (PS3)	f
2558	sbk09ps3am	\N	SBK '09  Automatch (PS3)	f
2559	sbk09pcam	\N	SBK '09  Automatch (PC)	f
2560	poriginpcjp	\N	Fear 2: Project Origin (JP) (PC)	f
2561	poriginpcjpam	\N	Fear 2: Project Origin  Automatch (JP) (PC)	f
2562	poriginpcjpd	\N	Fear 2: Project Origin  Demo (JP) (PC)	f
2563	poriginps3jp	\N	Fear 2: Project Origin (JP) (PS3)	f
2564	poriginps3jpam	\N	Fear 2: Project Origin  Automatch (JP) (PS3)	f
2565	poriginps3jpd	\N	Fear 2: Project Origin  Demo (JP) (PS3)	f
2566	section8pc	\N	Section 8 (PC)	f
2567	section8pcam	\N	Section 8  Automatch (PC)	f
2568	section8pcd	\N	Section 8  Demo (PC)	f
2569	section8ps3	\N	Section 8 (PS3)	f
2570	section8ps3am	\N	Section 8  Automatch (PS3)	f
2571	section8ps3d	\N	Section 8  Demo (PS3)	f
2572	section8x360	\N	Section 8 (Xbox360)	f
2573	section8x360am	\N	Section 8  Automatch (Xbox360)	f
2574	section8x360d	\N	Section 8  Demo (Xbox360)	f
2575	buccaneerpc	\N	Buccaneer (PC)	f
2576	buccaneerpcam	\N	Buccaneer  Automatch (PC)	f
2577	buccaneerpcd	\N	Buccaneer  Demo (PC)	f
2578	civ4coljp	5wddmt	Sid Meier's Civilization IV: Colonization (PC Japanese)	f
2579	civ4coljpam	5wddmt	Sid Meier's Civilization IV: Colonization  Automatch (PC Japanese)	f
2580	beateratorpsp	\N	Beaterator (PSP)	f
2581	beateratorpspam	\N	Beaterator Automatch (PSP)	f
2582	beateratorpspd	\N	Beaterator Demo (PSP)	f
2583	sonicrkords	\N	Sonic Rush Adventure (KOR) (DS)	f
2584	mmadnesswii	\N	Military Madness (WiiWare)	f
2585	chesschalwii	\N	Chess Challenge! (WiiWare)	f
2586	chesschalwiiam	\N	Chess Challenge!  Automatch (WiiWare)	f
2587	narutorev3wii	\N	Naruto Shippuden: Clash of Ninja Revolution 3 (Wii)	f
2588	decasport2wii	\N	Deca Sports 2 (Wii)	f
2589	suparobods	\N	Suparobo Gakuen (DS)	f
2590	gh4ghitswii	\N	Guitar Hero 4: Greatest Hits (Wii)	f
2591	simsraceEUds	\N	MySims Racing DS (EU) (DS)	f
2592	blockrushwii	\N	Blockrush! (WiiWare)	f
2593	simsraceJPNds	\N	MySims Racing DS (JPN) (DS)	f
2594	superv8pc	\N	Superstars V8 Racing (PC)	f
2595	superv8pcam	\N	Superstars V8 Racing  Automatch (PC)	f
2596	superv8pcd	\N	Superstars V8 Racing  Demo (PC)	f
2597	superv8ps3	\N	Superstars V8 Racing (PS3)	f
2598	superv8ps3am	\N	Superstars V8 Racing  Automatch (PS3)	f
2599	superv8ps3d	\N	Superstars V8 Racing  Demo (PS3)	f
2600	boardgamesds	\N	The Best of Board Games (DS)	f
2601	cardgamesds	\N	The Best of Card Games (DS)	f
2602	colcourseds	\N	Collision Course (DS)	f
2604	puyopuyo7ds	\N	PuyoPuyo 7 (DS/Wii)	f
2605	qsolace	\N	Quantum of Solace	f
2606	tcendwar	\N	Tom Clancy's EndWar	f
2607	kidslearnwii	\N	Kids Learning Desk (WiiWare)	f
2608	svsr10ps3	\N	WWE Smackdown vs. Raw 2010 (PS3)	f
2609	svsr10ps3am	\N	WWE Smackdown vs. Raw 2010  Automatch (PS3)	f
2610	svsr10ps3d	\N	WWE Smackdown vs. Raw 2010  Demo (PS3)	f
2611	svsr10x360	\N	WWE Smackdown vs. Raw 2010 (Xbox 360)	f
2612	svsr10x360am	\N	WWE Smackdown vs. Raw 2010  Automatch (Xbox 360)	f
2613	svsr10x360d	\N	WWE Smackdown vs. Raw 2010  Demo (Xbox 360)	f
2614	momo2010wii	\N	Momotaro Dentetsu 2010 Nendoban (Wii)	f
2615	cardherods	\N	Card Hero DSi (DS)	f
2616	cardherodsam	\N	Card Hero DSi  Automatch (DS)	f
2617	smball2iph	\N	Super Monkey Ball 2 (iPhone)	f
2618	smball2ipham	\N	Super Monkey Ball 2  Automatch (iPhone)	f
2619	smball2iphd	\N	Super Monkey Ball 2  Demo (iPhone)	f
2620	beateratoriph	\N	Beaterator (iPhone)	f
2621	beateratoripham	\N	Beaterator Automatch (iPhone)	f
2622	beateratoriphd	\N	Beaterator Demo (iPhone)	f
2623	conduitwii	\N	The Conduit (Wii)	f
2624	hookagainwii	\N	Hooked Again! (Wii)	f
2625	rfactory3ds	\N	Rune Factory 3 (DS)	f
2626	disneydev	\N	Disney Development/Testing	f
2627	disneydevam	\N	Disney Development/Testing Automatch	f
2628	sporearenads	\N	Spore Hero Arena (DS)	f
2629	treasurewldds	\N	Treasure World (DS)	f
2630	unowii	\N	UNO (WiiWare)	f
2631	mekurucawii	\N	Mekuruca (WiiWare)	f
2632	bderlandspc	\N	Borderlands (PC)	f
2633	bderlandspcam	\N	Borderlands Automatch (PC)	f
2634	bderlandspcd	\N	Borderlands Demo (PC)	f
2635	bderlandsps3	\N	Borderlands (PS3)	f
2636	bderlandsps3am	\N	Borderlands Automatch (PS3)	f
2637	bderlandsps3d	\N	Borderlands Demo (PS3)	f
2638	bderlandsx360	\N	Borderlands (360)	f
2639	bderlands360am	\N	Borderlands Automatch (360)	f
2640	bderlandsx360d	\N	Borderlands Demo (360)	f
2641	simsportsds	\N	MySims Sports (DS)	f
2642	simsportswii	\N	MySims Sports (Wii)	f
2644	arma2pc	\N	Arma II (PC)	f
2645	arma2pcam	\N	Arma II Automatch (PC)	f
2646	arma2pcd	\N	Arma II Demo (PC)	f
2647	rubikguidewii	\N	Rubik's Puzzle World: Guide (WiiWare)	f
2648	quizmagic2ds	\N	Quiz Magic Academy DS2 (DS)	f
2649	bandbrosEUds	\N	Daiggaso! Band Brothers DX (EU) (DS)	f
2650	swsnow2wii	\N	Shaun White Snowboarding 2 (Wii)	f
2651	scribnautsds	\N	Scribblenauts (DS)	f
2652	fifasoc10ds	\N	FIFA Soccer 10 (DS)	f
2653	foreverbl2wii	\N	Forever Blue 2 (Wii)	f
2654	namcotest	\N	Namco SDK Test	f
2655	namcotestam	\N	Namco SDK Test Automatch	f
2656	namcotestd	\N	Namco SDK Test Demo	f
2657	blindpointpc	\N	Blind Point (PC)	f
2658	blindpointpcam	\N	Blind Point  Automatch (PC)	f
2659	blindpointpcd	\N	Blind Point  Demo (PC)	f
2660	propocket12ds	\N	PowerPro-kun Pocket 12 (DS)	f
2661	seafarmwii	\N	Seafarm (WiiWare)	f
2662	dragquestsds	\N	Dragon Quest S (DSiWare)	f
2663	dawnheroesds	\N	Dawn of Heroes (DS)	f
2664	monhunter3wii	\N	Monster Hunter 3 (JPN) (Wii)	f
2665	appletest	\N	Apple SDK test	f
2666	appletestam	\N	Apple SDK test Automatch	f
2667	appletestd	\N	Apple SDK test Demo	f
2668	harbunkods	\N	Harlequin Bunko (DS)	f
2669	unodsi	\N	UNO (DSiWare)	f
2670	beaterator	\N	Beaterator (PSP/iphone)	f
2671	beateratoram	\N	Beaterator  Automatch (PSP/iphone)	f
2672	beateratord	\N	Beaterator  Demo (PSP/iphone)	f
2673	ragonlineKRds	\N	Ragunaroku Online DS (KOR) (DS)	f
2674	dragoncrwnwii	\N	Dragon's Crown (Wii)	f
2675	ascensionpc	\N	Ascension (PC)	f
2676	ascensionpcam	\N	Ascension Automatch (PC)	f
2677	ascensionpcd	\N	Ascension Demo (PC)	f
2678	swbfespsp	\N	Star Wars: Battlefront - Elite Squadron (PSP)	f
2679	swbfespspam	\N	Star Wars: Battlefront - Elite Squadron  Automatch (PSP)	f
2680	swbfespspd	\N	Star Wars: Battlefront - Elite Squadron  Demo (PSP)	f
2681	nba2k10wii	\N	NBA 2K10 (Wii)	f
2682	nhl2k10wii	\N	NHL 2K10 (Wii)	f
2683	mk9test	\N	Midway MK9 Test	f
2684	mk9testam	\N	Midway MK9 Test Automatch	f
2685	mk9testd	\N	Midway MK9 Test Demo	f
2686	kateifestds	\N	Katei Kyoshi Hitman Reborn DS Vongole Festival Online (DS)	f
2687	luminarc2EUds	\N	Luminous Arc 2 Will (EU) (DS)	f
2688	tatvscapwii	\N	Tatsunoko vs. Capcom Ultimate All Stars (Wii)	f
2689	petz09ds	\N	Petz Catz/Dogz/Hamsterz/Babiez 2009 (DS)	f
2690	rtlwsportswii	\N	RTL Winter Sports 2010 (Wii)	f
2691	tomenasawii	\N	Tomenasanner (WiiWare)	f
2692	luchalibrepc	\N	Lucha Libre AAA 2010 (PC)	f
2693	luchalibrepcam	\N	Lucha Libre AAA 2010  Automatch (PC)	f
2694	luchalibrepcd	\N	Lucha Libre AAA 2010  Demo (PC)	f
2695	luchalibreps3	\N	Lucha Libre AAA 2010 (PS3)	f
2696	luchalibreps3am	\N	Lucha Libre AAA 2010  Automatch (PS3)	f
2697	luchalibreps3d	\N	Lucha Libre AAA 2010  Demo (PS3)	f
2698	simsflyerswii	\N	MySims Flyers (Wii)	f
2699	ludicrouspc	\N	Ludicrous (PC)	f
2700	ludicrouspcam	\N	Ludicrous Automatch (PC)	f
2701	ludicrouspcd	\N	Ludicrous Demo (PC)	f
2702	ludicrousmac	\N	Ludicrous (MAC)	f
2703	ludicrousmacam	\N	Ludicrous Automatch (MAC)	f
2704	ludicrousmacd	\N	Ludicrous Demo (MAC)	f
2705	pbellumr1	\N	Parabellum Region 1 (PC)	f
2706	pbellumr2	\N	Parabellum Region 2 (PC)	f
2707	pbellumr3	\N	Parabellum Region 3 (PC)	f
2708	imaginejdds	\N	Imagine: Jewelry Designer (DS)	f
2709	imagineartds	\N	Imagine: Artist (DS)	f
2710	tvshwking2wii	\N	TV Show King 2 (WiiWare)	f
2711	sballrevwii	\N	Spaceball: Revolution (WiiWare)	f
2712	orderofwarpc	\N	Order of War (PC)	f
2713	orderofwarpcam	\N	Order of War  Automatch (PC)	f
2714	orderofwarpcd	\N	Order of War  Demo (PC)	f
2715	lbookofbigsds	\N	Little Book of Big Secrets (DS)	f
2716	scribnauteuds	\N	Scribblenauts (EU) (DS)	f
2717	buccaneer	\N	Buccaneer The Pursuit of Infamy	f
2718	kenteitvwii	\N	Kentei! TV Wii (Wii)	f
2719	yugioh5dwii	\N	Yu-Gi-Oh! 5D's Duel Simulator (Wii)	f
2720	fairyfightps3	\N	Fairytale Fights (PS3)	f
2721	fairyfightps3am	\N	Fairytale Fights Automatch (PS3)	f
2722	fairyfightps3d	\N	Fairytale Fights Demo (PS3)	f
2723	fairyfightpc	\N	Fairytale Fights (PC)	f
2724	fairyfightpcam	\N	Fairytale Fights Automatch (PC)	f
2725	fairyfightpcd	\N	Fairytale Fights Demo (PC)	f
2726	50centjpnps3	\N	50 Cent: Blood on the Sand (JPN) (PS3)	f
2727	50centjpnps3am	\N	50 Cent: Blood on the Sand  Automatch (JPN) (PS3)	f
2728	50centjpnps3d	\N	50 Cent: Blood on the Sand  Demo (JPN) (PS3)	f
2729	codmw2ds	\N	Call of Duty: Modern Warfare 2 (DS)	f
2730	jbond2009ds	\N	James Bond 2009 (DS)	f
2731	resevildrkwii	\N	Resident Evil: The Darkside Chronicles (Wii)	f
2732	musicmakerwii	\N	Music Maker (Wii)	f
2733	figlandds	\N	Figland (DS)	f
2734	bonkwii	\N	Bonk (Wii)	f
2735	bomberman2wii	\N	Bomberman 2 (Wii)	f
2736	bomberman2wiid	\N	Bomberman 2  Demo (Wii)	f
2737	dreamchronwii	\N	Dream Chronicle (Wii)	f
2738	gokuidsi	\N	Gokui (DSiWare)	f
2739	usingwii	\N	U-Sing (Wii)	f
2740	shikagariwii	\N	Shikagari (Wii)	f
2741	puyopuyo7wii	\N	Puyopuyo 7 (Wii)	f
2742	winelev10wii	\N	Winning Eleven Play Maker 2010 (Wii)	f
2743	section8pcb	\N	Section 8 Beta (PC)	f
2744	section8pcbam	\N	Section 8 Beta  Automatch (PC)	f
2745	section8pcbd	\N	Section 8 Beta  Demo (PC)	f
2746	ubraingamesds	\N	Ultimate Brain Games (DS)	f
2747	ucardgamesds	\N	Ultimate Card Games (DS)	f
2748	postpetds	\N	PostPetDS Yumemiru Momo to Fushigi no Pen (DS)	f
2749	mfightbbultds	\N	Metal Fight Bay Blade ULTIMATE (DS)	f
2750	strategistwii	\N	Strategist (Wii)	f
2751	bmbermanexdsi	\N	Bomberman Express (DSiWare)	f
2752	blockoutwii	\N	Blockout (Wii)	f
2753	rdr2x360	\N	Red Dead Redemption (x360)	f
2754	rdr2x360am	\N	Red Dead Redemption  Automatch (x360)	f
2757	fairyfightspc	\N	Fairytale Fights (PC)	f
2758	fairyfightspcam	\N	Fairytale Fights  Automatch (PC)	f
2759	fairyfightspcd	\N	Fairytale Fights  Demo (PC)	f
2760	stalkercoppc	\N	STALKER: Call of Pripyat (PC)	f
2761	stalkercoppcam	\N	STALKER: Call of Pripyat  Automatch (PC)	f
2762	stalkercoppcd	\N	STALKER: Call of Pripyat  Demo (PC)	f
2763	strategistpc	\N	The Strategist (PC)	f
2764	strategistpcam	\N	The Strategist Automatch (PC)	f
2765	strategistpcd	\N	The Strategist Demo (PC)	f
2766	strategistpsn	\N	The Strategist (PSN)	f
2767	strategistpsnam	\N	The Strategist Automatch (PSN)	f
2768	strategistpsnd	\N	The Strategist Demo (PSN)	f
2769	tataitemogwii	\N	Tataite! Mogumon US/EU (WiiWare)	f
2770	ufc10ps3	\N	UFC 2010 (PS3)	f
2771	ufc10ps3am	\N	UFC 2010 Automatch (PS3)	f
2772	ufc10ps3d	\N	UFC 2010 Demo (PS3)	f
2773	ufc10x360	\N	UFC 2010 (x360)	f
2774	ufc10x360am	\N	UFC 2010 Automatch (x360)	f
2775	ufc10x360d	\N	UFC 2010 Demo (x360)	f
2776	mmtest	\N	Matchmaking Backend Test	f
2777	mmtestam	\N	Matchmaking Backend Test Automatch	f
2778	talesofgrawii	\N	Tales of Graces (Wii)	f
2779	dynamiczanwii	\N	Dynamic Zan (Wii)	f
2780	fushigidunds	\N	Fushigi no Dungeon Furai no Shiren 4 Kami no Me to Akama no Heso (DS)	f
2781	idraculawii	\N	iDracula (WiiWare)	f
2782	metalfightds	\N	Metal Fight Bayblade (DS)	f
2783	wormswiiware	\N	Worms (WiiWare)	f
2784	wormswiiwaream	\N	Worms Automatch (WiiWare)	f
2785	justsingds	\N	Just Sing! (DS)	f
2786	gtacwarspsp	\N	Grand Theft Auto: Chinatown Wars (PSP)	f
2787	gtacwarspspam	\N	Grand Theft Auto: Chinatown Wars  Automatch (PSP)	f
2788	gtacwarspspd	\N	Grand Theft Auto: Chinatown Wars  Demo (PSP)	f
2789	gtacwiphone	\N	Grand Theft Auto: Chinatown Wars (iPhone)	f
2790	gtacwiphoneam	\N	Grand Theft Auto: Chinatown Wars  Automatch (iPhone)	f
2791	gtacwiphoned	\N	Grand Theft Auto: Chinatown Wars  Demo (iPhone)	f
2792	trkmaniads	\N	Trackmania (DS)	f
2793	trkmaniawii	\N	Trackmania (Wii)	f
2794	megaman10wii	\N	Mega Man 10 (WiiWare)	f
2795	aarmy3	\N	America's Army 3	f
2796	tycoonnyc	\N	Tycoon City - New York	f
2797	sinpunish2wii	\N	Sin & Punishment 2 (Wii)	f
2798	fuelps3ptchd	\N	FUEL (PS3) Patched version	f
2799	fuelps3ptchdam	\N	FUEL  Automatch (PS3) Patched version	f
2800	sonicdlwii	\N	Sonic DL (WiiWare)	f
2801	demonforgeps3	\N	Demon's Forge (PS3)	f
2802	demonforgeps3am	\N	Demon's Forge  Automatch (PS3)	f
2803	demonforgeps3d	\N	Demon's Forge  Demo (PS3)	f
2804	demonforgepc	\N	Demon's Forge (PC)	f
2805	demonforgepcam	\N	Demon's Forge  Automatch (PC)	f
2806	demonforgepcd	\N	Demon's Forge  Demo (PC)	f
2807	hooploopwii	\N	HooperLooper (WiiWare)	f
2809	test1	\N	test1	f
2810	maxpayne3pc	\N	Max Payne 3 (PC)	f
2811	maxpayne3pcam	\N	Max Payne 3 Automatch (PC)	f
2812	maxpayne3pcd	\N	Max Payne 3 Demo (PC)	f
2813	maxpayne3ps3	\N	Max Payne 3 (PS3)	f
2814	maxpayne3ps3am	\N	Max Payne 3 Automatch (PS3)	f
2815	maxpayne3ps3d	\N	Max Payne 3 Demo (PS3)	f
2816	maxpayne3x360	\N	Max Payne 3 (360)	f
2817	maxpayne3x360am	\N	Max Payne 3 Automatch (360)	f
2818	maxpayne3x360d	\N	Max Payne 3 Demo (360)	f
2819	wordjongeuds	\N	Wordjong EU (DS)	f
2820	sengo3wii	\N	Sengokumuso 3	f
2821	bewarewii	\N	Beware (WiiWare)	f
2822	hinterland	\N	Hinterland	f
2823	hastpaint2wii	\N	Greg Hastings Paintball 2 (Wii)	f
2824	rockstarsclub	\N	Rockstar Social Club	f
2825	rockstarsclubam	\N	Rockstar Social Club Automatch	f
2826	plandmajinds	\N	Professor Layton and Majin no Fue (DS)	f
2827	powerkoushds	\N	Powerful Koushien (DS)	f
2828	cavestorywii	\N	Cave Story (WiiWare)	f
2829	blahblahtest	\N	Just another test for masterid	f
2830	blahtest	\N	Just another test for masterid	f
2831	blahmasterid	\N	Just another test for masterid	f
2832	bädmasterid	\N	bädmasterid	f
2833	explomäntest	\N	blah	f
2836	3dpicrosseuds	\N	3D Picross (EU) (DS)	f
2837	gticsfestwii	\N	GTI Club Supermini Festa (Wii)	f
2838	narutor3euwii	\N	Naruto Shippuden: Clash of Ninja Revolution 3 EU (Wii)	f
2840	sparta2pc	\N	Sparta 2: The Conquest of Alexander the Great (PC)	f
2841	sparta2pcam	\N	Sparta 2: The Conquest of Alexander the Great  Automatch (PC)	f
2842	sparta2pcd	\N	Sparta 2: The Conquest of Alexander the Great  Demo (PC)	f
2843	superv8ncpc	\N	Superstars V8 Next Challenge (PC)	f
2844	superv8ncpcam	\N	Superstars V8 Next Challenge  Automatch (PC)	f
2845	superv8ncpcd	\N	Superstars V8 Next Challenge  Demo (PC)	f
2846	superv8ncps3	\N	Superstars V8 Next Challenge (PS3)	f
2847	superv8ncps3am	\N	Superstars V8 Next Challenge  Automatch (PS3)	f
2848	superv8ncps3d	\N	Superstars V8 Next Challenge  Demo (PS3)	f
2849	ikaropc	\N	Ikaro (PC)	f
2850	ikaropcam	\N	Ikaro  Automatch (PC)	f
2851	ikaropcd	\N	Ikaro  Demo (PC)	f
2852	ufc10ps3DEV	\N	UFC 2010 DEV (PS3-DEV)	f
2853	ufc10ps3DEVam	\N	UFC 2010 DEV  Automatch (PS3-DEV)	f
2854	ufc10ps3DEVd	\N	UFC 2010 DEV  Demo (PS3-DEV)	f
2855	ufc10x360dev	\N	UFC 2010 DEV (360-DEV)	f
2856	ufc10x360devam	\N	UFC 2010 DEV  Automatch (360-DEV)	f
2857	ufc10x360devd	\N	UFC 2010 DEV  Demo (360-DEV)	f
2858	ragonlinenads	\N	Ragunaroku Online DS (NA) (DS)	f
2859	hoopworldwii	\N	Hoopworld (Wii)	f
2860	foxtrotpc	\N	Foxtrot (PC)	f
2861	foxtrotpcam	\N	Foxtrot  Automatch (PC)	f
2862	foxtrotpcd	\N	Foxtrot  Demo (PC)	f
2863	civ5	\N	Civilization 5	f
2864	heroeswii	\N	Heroes (Wii)	f
2865	yugiohwc10ds	\N	Yu-Gi-Oh! World Championship 2010 (DS)	f
2866	sbkxpc	\N	SBK X: Superbike World Championship (PC)	f
2867	sbkxpcam	\N	SBK X: Superbike World Championship  Automatch (PC)	f
2868	sbkxpcd	\N	SBK X: Superbike World Championship  Demo (PC)	f
2869	sbkxps3	\N	SBK X: Superbike World Championship (PS3)	f
2870	sbkxps3am	\N	SBK X: Superbike World Championship  Automatch (PS3)	f
2871	sbkxps3d	\N	SBK X: Superbike World Championship  Demo (PS3)	f
2872	famista2010ds	\N	Famista 2010 (DS)	f
2873	bokutwinvilds	\N	Bokujyo Monogatari Twin Village (DS)	f
2874	destruction	\N	Destruction 101 (Namco Bandai)	f
2875	destructionam	\N	Destruction 101 Automatch	f
2876	lumark3eyesds	\N	Luminous Ark 3 Eyes (DS)	f
2877	othellowii	\N	Othello (WiiWare)	f
2878	painkresurrpc	\N	Painkiller Resurrection (PC)	f
2879	painkresurrpcam	\N	Painkiller Resurrection  Automatch (PC)	f
2880	painkresurrpcd	\N	Painkiller Resurrection  Demo (PC)	f
2881	fantcubewii	\N	Fantastic Cube (WiiWare)	f
2882	3dpicrossUSds	\N	3D Picross (US) (DS)	f
2883	svsr11ps3	\N	Smackdown vs Raw 2011 (PS3)	f
2884	svsr11ps3am	\N	Smackdown vs Raw 2011  Automatch (PS3)	f
2885	svsr11ps3d	\N	Smackdown vs Raw 2011  Demo (PS3)	f
2886	svsr11x360	\N	Smackdown vs Raw 2011 (x360)	f
2887	svsr11x360am	\N	Smackdown vs Raw 2011  Automatch (x360)	f
2888	svsr11x360d	\N	Smackdown vs Raw 2011  Demo (x360)	f
2889	bderlandruspc	\N	Borderlands RUS (PC)	f
2890	bderlandruspcam	\N	Borderlands RUS  Automatch (PC)	f
2891	bderlandruspcd	\N	Borderlands RUS  Demo (PC)	f
2892	krabbitpcmac	\N	KrabbitWorld Origins (PC/Mac)	f
2893	krabbitpcmacam	\N	KrabbitWorld Origins  Automatch (PC/Mac)	f
2894	krabbitpcmacd	\N	KrabbitWorld Origins  Demo (PC/Mac)	f
2895	gunnylamacwii	\N	GUNBLADE NY & L.A. MACHINEGUNS (Wii)	f
2896	rbeaverdefwii	\N	Robocalypse - Beaver Defense (WiiWare)	f
2897	surkatamarwii	\N	Surinukeru Katamari (WiiWare)	f
2898	snackdsi	\N	Snack (DSiWare)	f
2899	rpgtkooldsi	\N	RPG tkool DS (DSi)	f
2900	mh3uswii	\N	Monster Hunter 3 (US/EU) (Wii)	f
2901	lanoireps3	\N	L.A. Noire (PS3)	f
2902	lanoireps3am	\N	L.A. Noire  Automatch (PS3)	f
2903	lanoireps3d	\N	L.A. Noire  Demo (PS3)	f
2904	lanoirex360	\N	L.A. Noire (x360)	f
2905	lanoirex360am	\N	L.A. Noire  Automatch (x360)	f
2906	lanoirex360d	\N	L.A. Noire  Demo (x360)	f
2907	lanoirepc	\N	L.A. Noire (PC)	f
2908	lanoirepcam	\N	L.A. Noire  Automatch (PC)	f
2909	lanoirepcd	\N	L.A. Noire  Demo (PC)	f
2910	digimonsleds	\N	Digimon Story Lost Evolution (DS)	f
2911	syachi2ds	\N	syachi 2 (DS)	f
2912	puzzleqt2ds	\N	Puzzle Quest 2 (DS)	f
2913	phybaltraiwii	\N	Physiofun Balance Trainer (WiiWare)	f
2914	decasport3wii	\N	Deca Sports 3 (Wii)	f
2915	tetrisdeluxds	\N	Tetris Party Deluxe (DSiWare)	f
2916	gsiphonefw	\N	GameSpy iPhone Framework	f
2917	necrolcpc	\N	NecroVisioN: Lost Company (PC)	f
2918	necrolcpcam	\N	NecroVisioN: Lost Company  Automatch (PC)	f
2919	necrolcpcd	\N	NecroVisioN: Lost Company  Demo (PC)	f
2920	startrekmac	\N	Star Trek: D-A-C (MAC)	f
2921	startrekmacam	\N	Star Trek  Automatch (MAC)	f
2922	captsubasads	\N	Captain tsubasa (DS)	f
2923	cb2ds	\N	CB2 (DS)	f
2924	katekyohitds	\N	katekyo hitman REBORN! DS FLAME RUMBLE XX (DS)	f
2925	cardiowrk2wii	\N	Cardio Workout 2 (Wii)	f
2926	boyvgirlcwii	\N	Boys vs Girls Summer Camp (Wii)	f
2927	keenracerswii	\N	Keen Racers (WiiWare)	f
2928	scribnaut2pc	\N	Scribblenauts 2 (PC)	f
2929	scribnaut2pcam	\N	Scribblenauts 2  Automatch (PC)	f
2930	agentps3	\N	Agent (PS3)	f
2931	agentps3am	\N	Agent  Automatch (PS3)	f
2932	girlskoreads	\N	Girls_Korea (DS)	f
2933	jyankenparwii	\N	Jyanken (rock-paper-scissors) Party Paradise (WiiWare)	f
2934	protocolwii	\N	Protocol (WiiWare)	f
2935	DeathtoSpies	\N	Death to Spies	f
2936	svsr11x360dev	\N	Smackdown vs Raw 2011 DEV (x360)	f
2937	svsr11x360devam	\N	Smackdown vs Raw 2011 DEV  Automatch (x360)	f
2938	svsr11ps3dev	\N	Smackdown vs Raw 2011 DEV (PS3)	f
2939	svsr11ps3devam	\N	Smackdown vs Raw 2011 DEV  Automatch (PS3)	f
2940	dynaztrialwii	\N	Dynamic Zan TRIAL (Wii)	f
2941	molecontrolpc	\N	Mole Control (PC)	f
2942	molecontrolpcam	\N	Mole Control  Automatch (PC)	f
2943	sakwcha2010ds	\N	Sakatsuku DS WorldChallenge 2010 (DS)	f
2944	MenofWar	\N	Men of War	f
2945	na2rowpc	\N	NAT2 Row (PC)	f
2946	na2rowpcam	\N	NAT2 Row  Automatch (PC)	f
2947	na2runpc	\N	NAT2 Run (PC)	f
2948	na2runpcam	\N	NAT2 Run  Automatch (PC)	f
2949	trackmania2ds	\N	Trackmania DS 2 (DS)	f
2950	pangmagmichds	\N	Pang: Magical Michael (DS)	f
2951	mysimsflyerds	\N	MySims Flyers (DS)	f
2952	mysimsflyEUds	\N	MySims Flyers EU (DS)	f
2953	kodawar2010ds	\N	Kodawari Saihai Simulation Ochanoma Pro Yakyu DS 2010 Verison (DS)	f
2954	topspin4wii	\N	TOPSPIN 4 (Wii)	f
2955	ut3onlive	\N	Unreal Tournament 3 ONLIVE	f
2956	ut3onliveam	\N	Unreal Tournament 3 ONLIVE Automatch	f
2957	combatzonepc	\N	Combat Zone - Special Forces (PC)	f
2958	combatzonepcam	\N	Combat Zone - Special Forces  Automatch (PC)	f
2959	combatzonepcd	\N	Combat Zone - Special Forces  Demo (PC)	f
2960	sinpun2NAwii	\N	Sin & Punishment 2 NA (Wii)	f
2962	capricornam	\N	Crysis 2 Automatch (PC)	f
2963	crysis2pcd	\N	Crysis 2 Demo (PC)	f
2964	crysis2ps3	\N	Crysis 2 (PS3)	f
2965	crysis2ps3am	\N	Crysis 2 Automatch (PS3)	f
2966	crysis2ps3d	\N	Crysis 2 Demo (PS3)	f
2967	crysis2x360	\N	Crysis 2 (Xbox 360)	f
2968	crysis2x360am	\N	Crysis 2 Automatch (Xbox 360)	f
2969	crysis2x360d	\N	Crysis 2 Demo (Xbox 360)	f
2970	ZumaDeluxe	\N	Zuma Deluxe	f
2971	cellfacttwpc	\N	Cell Factor:TW (PC)	f
2972	cellfacttwpcam	\N	Cell Factor:TW  Automatch (PC)	f
2973	firearmsevopc	\N	Firearms Evolution (PC)	f
2974	firearmsevopcam	\N	Firearms Evolution  Automatch (PC)	f
2975	winel10jpnwii	\N	Winning Eleven PLAY MAKER 2010 Japan Edition (Wii)	f
2976	winel10jpnwiiam	\N	Winning Eleven PLAY MAKER 2010 Japan Edition  Automatch (Wii)	f
2977	bldragonNAds	\N	Blue Dragon - Awakened Shadow	f
2978	bldragonNAdsam	\N	Blue Dragon - Awakened Shadow Automatch	f
2979	sonic2010wii	\N	SONIC 2010 (Wii)	f
2980	sonic2010wiiam	\N	SONIC 2010  Automatch (Wii)	f
2981	harmoon2kords	\N	Harvest Moon 2 Korea (DS)	f
2982	harmoon2kordsam	\N	Harvest Moon 2 Korea  Automatch (DS)	f
2983	jbondmv2ds	\N	James Bond Non Movie 2 (2010) (DS)	f
2984	jbondmv2dsam	\N	James Bond Non Movie 2  Automatch (2010) (DS)	f
2985	casinotourwii	\N	Casino Tournament (Wii)	f
2986	casinotourwiiam	\N	Casino Tournament  Automatch (Wii)	f
3300	capricorn	8TTq4M	Crysis 2 (PC)	f
\.


--
-- TOC entry 3447 (class 0 OID 16405)
-- Dependencies: 216
-- Data for Name: grouplist; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.grouplist (groupid, gameid, roomname) FROM stdin;
1	5	daikatana test group
2	1	Newbies
3	1	Experts
4	1	Farm Animals
5	256	Skirmish
6	256	Domination
7	192	Test VP3 Tourney
9	192	this
18	192	b
19	192	b
20	285	Rookies
21	285	Amateurs
22	285	Pros
24	192	b
25	256	Slaughter
26	256	Soul Harvest
27	256	Allied
30	192	LumberJack VP3 Test Tourney
44	192	test6ladder
57	192	asdf
63	192	BillsTest2
64	192	BillsTest2
101	192	this
102	308	Beginner
103	308	Intermediate
104	308	Advanced
105	192	this
106	192	hello
107	192	mytest
108	192	arts ladder
109	192	Seans Ladder
110	192	seans test ladder
111	192	Seans Test Ladder
112	192	9-Ball Challenge
113	192	TestOct23
114	192	abcd
115	192	Lumberjack VP3 Test Tourny #2
116	192	9-Ball Heaven
117	192	QA Test Ladder
118	192	GSI Test - do not join
119	192	reload test
122	192	Tonys 9-Ball tourney
123	192	Tonys 9-Ball tourney 2
124	192	Tonys Moved Database Test
125	192	outputdir test tourney
127	192	b
141	192	aphexweb1 test
143	192	registration test
144	192	w
147	192	arts ladder test
148	192	gsi test ladder nov 1
149	192	test
150	192	arts ladder
151	192	gsi test ladder
152	192	tonys ladder
153	192	9-Ball Ladder: Public Test
154	192	Interplay QA test
155	192	Interplay QA test
156	192	Qa Test 2
157	192	QA DEDICATED TEST
158	192	TEN BALL - QA TEST
159	192	BILLIARDS
160	192	IP Rotation
161	192	10 BALL - QA TEST
162	192	Savy & Sean
163	192	Willow & Erik
164	192	Savy
165	192	6 Ball game
166	192	VP3 Private Patch Testing
167	192	Patch (#2) Final Testing
168	337	Main Lobby
169	337	{01}Cadet
170	337	{02}Captain
171	337	{03}Admiral
172	412	{01}General Chat
173	412	{04}Teen Chat
176	412	{02}Family And Friends
177	412	{03}College Chat
189	15	{01}Half-Life Room 1
190	15	{02}Half-Life Room 2
191	15	{01}Counter-Strike: Special Air Service
192	15	{01}Counter-Strike: GSG-9
193	15	{01}Counter-Strike: Counter-Terrorist Force
194	15	{01}Counter-Strike: Seal Team 6
195	15	{11}Firearms Room
196	15	{13}TeamFortress Classic Room
197	15	{10}Day of Defeat Room
198	15	{12}Front Line Force Room
199	412	{01}Action Games
200	412	{02}Role Playing Games
201	412	{03}Strategy Games
202	412	{04}Sports Games
203	412	{05}Simulation Games
204	412	{06}Tactical Games
206	15	{03}Help With Half-Life
207	323	{01}Counter-Terrorist Force
208	323	{01}Seal Team Six
209	323	{02}Help with Counter-Strike
210	22	{02}Quake 3 Veterans Room
211	22	{01}Quake 3 Main Room
212	22	{05}Rocket Arena 3 Room
213	22	{02}Freeze Tag
214	22	{04}Quake 3 Fortress Room
215	22	{06}Threewave CTF
216	22	{07}Urban Terror Beta 2 Room
217	22	{08}Weapons Factory Arena Room
218	22	{01}Excessive Room
219	401	{03}Spades Advanced Lobby
220	401	{01}Spades Newbie Lobby
221	401	{04}Spades Ranked Lobby
222	401	{02}Spades Social Lobby
223	403	{02}Backgammon Ranked Lobby
224	403	{01}Backgammon General Lobby
225	400	Poker Advanced Lobby
226	400	Poker Newbies Lobby
227	400	Poker Ranked Lobby
228	400	Poker Social Lobby
229	402	{01}Hearts General Lobby
230	402	{02}Hearts Ranked Lobby
231	22	{06}Help with Quake 3
232	22	{05}Team Deathmatch
233	15	{01}Counter-Strike: For Great Justice!
234	15	{01}Counter-Strike: Clan Battle Room
236	15	{13}Action Half-Life Room
241	15	{03}Opposing Force Room
242	15	{04}PlanetHalfLife Arcade Event Lobby
243	401	{05}Spades Tournament Lobby
244	402	{03}Hearts Tournament Lobby
245	403	{03}Backgammon Tournament Lobby
246	192	Squish Test ladder
247	292	Main
248	292	Tournaments
250	361	{01}Geral
251	361	{02}Jogos PC
252	15	{14}Deathmatch Classic
259	414	{01}Everon
260	414	{01}Malden
261	414	{02}Dedicated Servers
262	414	{03}Co-op
263	414	{04}Capture the Flag
264	414	{05}Test Zone
265	361	{03}Jogos Consolas
266	361	{04}Hardware
267	361	{05}Torneios e Eventos
269	361	{06}Comunidade
270	483	THPS3 Internet
271	400	Poker Tournament Lobby
272	22	{03}Orange Smoothie
273	504	Main
274	328	{01}Action
275	328	{02}Roleplaying
276	328	{03}Team (N vs. N)
277	328	{04}Social
278	328	{05}Persistent World Action
279	328	{06}Alternative
280	292	Korean
281	509	The Downs
288	509	Forest Heart
295	509	Tharsis
299	523	Main Lobby
300	523	{01}Cadet
301	523	{02}Captain
302	523	{03}Admiral
303	22	{03}Capture the Flag
304	22	{04}Deathmatch
305	292	Français
306	292	Deutsch
307	292	KIS
308	292	KAG
310	362	{01}Juegos
311	362	{02}Adolescentes
312	362	{03}Encuentros
313	362	{04}Maduritos
314	362	{05}Sexo
315	564	Tony Hawk 2x
316	564	Halo
317	564	NASCAR Heat 2002
318	568	{01}Eastern Front
319	568	{01}Western Front
320	568	{01}North African Campaign
321	568	{01}Pacific Campaign
322	568	{01}Guadalcanal Campaign
323	568	{01}Soviet Winter Offensive
324	363	{01}Juegos
325	363	{02}Adolescentes
326	363	{03}Encuentros
327	363	{04}Maduritos
328	363	{05}Romance
329	492	{01}Eastern Front
330	492	{01}Western Front
331	492	{02}Round-based Match
332	492	{04}Objective-based Match
333	492	{01}Deathmatch
334	492	{03}Team Match
335	492	{01}North Africa
336	492	{01}Siegfried Line
337	492	{03}Team Match
338	492	{04}Objective-based Match
339	590	(01)Power Plant
340	590	(01)Tiberium Refinery
341	590	(01)Weapons Factory
342	590	(01)Infantry Barracks
343	590	(01)GDI Guard Tower
344	590	(01)Construction Yard
345	590	(01)Hand of Nod
346	590	(01)Nod Airstrip
347	15	{15}Desert Crisis
352	577	(01)Power Plant
353	577	(01)Tiberium Refinery
354	577	(01)Weapons Factory
355	577	(01)Infantry Barracks
356	577	(01)GDI Guard Tower
357	577	(01)Construction Yard
358	577	(01)Hand of Nod
359	577	(01)Nod Airstrip
361	564	Tony Hawk 3
362	564	MotoGP
363	328	{07}Story
364	328	{08}Story Lite
365	328	{09}Melee (1 vs. N)
366	328	{10}Arena (1 vs. N)
367	328	{11}Persistent World Story
368	328	{12}Solo
369	412	{05}Romance
370	328	{13}Tech Support
371	15	{16}Ricochet
372	564	Australia
373	564	Europe
374	564	United Kingdom
375	610	{01}Infantry
376	610	{01}Combat Engineering
377	610	{01}Combat Operations
378	610	{01}Special Forces
379	610	{01}Armor
380	610	{01}Aviation Operations
381	610	{05}MOUT McKenna
382	610	{05}MOUT McKenna
383	610	{01}Bridge Crossing
384	610	{03}Headquarters Raid
385	610	{04}Insurgent Camp
386	610	{06}Pipeline
387	610	{02}Collapsed Tunnel
388	610	{01}Bridge Crossing
389	675	GroupRoom1
390	675	GroupRoom2
391	675	GroupRoom3
392	675	QuickMatch
393	564	NFL Fever 2003
394	684	Los Angeles (Newbies)
395	684	Los Angeles (Experts)
396	684	Tokyo (Newbies)
397	684	Tokyo (Experts)
398	684	Paris (Newbies)
399	684	Paris (Experts)
400	684	Battle (Newbies)
401	684	Battle (Experts)
402	671	(01)Allies Lobby
403	671	(01)Allies Lobby
404	671	(01)Axis Lobby
405	671	(01)Axis Lobby
409	617	Rookie
410	617	Intermediate
411	617	Expert
412	541	{01}Axis Lobby
413	541	{01}Allies Lobby
414	541	{03}Pacific Theater
415	541	{02}European Theater
417	541	{04}Russian Theater
418	541	Capture the Flag
419	541	Conquest
420	541	Co-Op
421	541	Team Deathmatch
422	541	{01}African Theater
423	636	Main Lobby
424	636	ATI Tournament Lobby
441	564	TimeSplitters 2
442	564	Tony Hawk 4
443	15	{17}Natural Selection
444	716	GroupRoom1
445	716	GroupRoom2
446	716	GroupRoom3
447	564	Deathrow
448	712	{01}General Lobby
449	712	{01}General Lobby
450	712	Free-For-All Servers
451	712	Team Deathmatch
452	712	Round-based Match
453	712	Objective Match
454	712	Tug of War
455	641	{01}General Lobby
456	641	{01}General Lobby
457	641	Free-For-All Servers
458	641	Team Deathmatch
459	641	Round-based Match
460	641	Objective Match
461	641	Tug of War
462	564	MechAssault
463	564	Unreal Championship
464	564	Ghost Recon
471	617	Rated
472	617	Unrated
473	617	Unrated Expert
474	617	Unrated Intermediate
475	617	Unrated Beginner
476	730	Beginner
477	730	Intermediate
478	730	Advanced
479	721	Rated 0
480	721	Rated 1
481	721	Rated 2
483	721	Rated 3
485	557	Ryan's Room
486	557	Donnie's Room
487	541	Desert Combat
488	713	{01}Allies Lobby
489	713	{01}Axis Lobby
490	713	{01}African Theater
491	713	{02}European Theater
492	713	{03}Italian Theater
493	713	{04}Pacific Theater
494	713	{05}Russian Theater
496	675	GroupRoom4
497	675	GroupRoom5
498	675	GroupRoom6
499	675	GroupRoom7
500	675	GroupRoom8
501	675	GroupRoom9
502	675	GroupRoom10
503	675	GroupRoom11
504	675	GroupRoom12
506	557	Kyle's Room
507	557	Bobby's Room
508	557	Nick's Room
509	557	Linda's Room
510	557	Melanie's Room
511	557	Cannonball's Room
512	557	Paulie's Room
513	557	Cuban Joe's Room
514	541	Galactic Conquest
515	770	Ryan's Room
516	770	Bobby's Room
517	722	Anything Goes
519	722	Team17
523	722	Professional League
524	722	Elite League
525	721	Rated 4
527	721	Rated 6
528	721	Rated 7
529	721	Rated 8
530	721	Rated 9
531	721	Unrated 0
532	721	Unrated 1
533	721	Unrated 2
534	721	Unrated 3
535	721	Unrated 4
536	721	Rated 5
537	721	Unrated 5
538	721	Unrated 6
539	721	Unrated 7
540	721	Unrated 8
541	721	Unrated 9
542	765	tier1
543	765	tier2
544	765	tier3
545	765	tier4
546	765	tier5
547	776	West
548	776	East
549	776	Europe
550	776	Asia
551	776	Beginner
552	776	Expert
553	15	{18}Vampire Slayer
554	772	Action Room
555	772	Asian Room
556	772	Deathmatch Room
557	797	{01}Main Lobby
558	797	{03}Tour Lobby
559	797	{02}Ladder Lobby
560	564	Wolfenstein Tides of War
561	564	Brute Force
562	541	Eve of Destruction
563	564	Midnight Club 2
564	564	Moto GP 2
565	564	Inside Pitch 2003
566	564	Star Wars: The Clone Wars
567	564	Midtown Madness 3
568	541	ActionBattlefield
569	792	Conquest Scenarios
570	792	Battle Scenarios
571	823	GroupRoom1
572	824	Unrated 0
573	824	Unrated 1
575	824	Unrated 2
576	824	Unrated 3
577	824	Unrated 4
578	824	Unrated 5
579	824	Unrated 6
580	824	Unrated 7
581	824	Unrated 8
582	824	Unrated 9
586	823	GroupRoom2
587	823	GroupRoom3
588	823	GroupRoom4
589	823	GroupRoom5
590	823	GroupRoom6
591	823	GroupRoom7
592	823	GroupRoom8
593	823	GroupRoom9
594	823	GroupRoom10
595	823	GroupRoom11
596	823	GroupRoom12
597	823	QuickMatch
598	823	GroupRoom13
599	840	Advanced
600	840	Intermediate
601	840	Beginner
602	823	GroupRoom14
606	832	Social Room
607	832	Beginner Room
608	832	Intermediate Room
609	832	Advanced Room
610	851	West
611	851	East
612	851	Europe
613	851	Asia
614	851	Beginner
615	851	Expert
616	22	{09}Urban Terror Beta 3 Room
619	15	{19}The Specialists
620	15	{20}MonkeyStrike
621	15	{21}Earth Special Forces
622	722	Amateur League
623	722	Shopping
624	842	Beginner
625	842	Casual
626	842	Expert
627	842	Elite
628	832	Practice Room
631	871	Newbies
632	871	Pros
633	871	Moto 1
634	871	Moto 2
635	845	Zaramoth
636	845	Zaramoth
637	845	Azunai
638	845	Azunai
639	845	Xeria
640	845	Xeria
641	845	Isteru
642	772	Empire Builder Room
643	772	European Room
644	772	Free For All Room
645	772	German Room
646	772	Ladder Room
647	772	No Rush Room
648	772	Tournament Room
649	843	European Public League
650	843	Massive Test Leauge
651	843	North American Public League
652	843	Asian Public League
660	870	LobbyRoom1
661	870	LobbyRoom2
662	870	QuickMatch
664	891	Group Room 2
665	564	Amped 2
666	564	Crimson Skies
667	564	NFL Fever 2004
668	564	Soldier of Fortune II
669	564	Ghost Recon: Island Thunder
670	564	Rainbow Six 3
671	564	Tony Hawk Underground
672	564	Top Spin
673	791	Conquest Scenarios
674	791	Battle Scenarios
675	15	{23}Counter-Strike: Clan Battle Room
676	15	{23}Counter-Strike: For Great Justice!
677	15	{23}Counter-Strike: GSG-9
678	15	{23}Counter-Strike: Counter-Terrorist Force
679	15	{23}Counter-Strike: Seal Team 6
680	15	{23}Counter-Strike: Special Air Service
684	793	{01}Main Lobby
685	793	{02}Halo Tournament
696	886	Main
697	886	Tournament
698	868	Main
699	868	Tournament
715	852	Search and Destroy
716	852	Behind Enemy Lines
717	852	Retrieval
718	852	Deathmatch
719	924	Rated 0
720	924	Rated 1
721	924	Rated 2
722	924	Rated 3
723	924	Rated 4
724	924	Rated 5
725	924	Rated 6
726	924	Rated 7
727	924	Rated 8
728	924	Rated 9
729	924	Unrated 0
730	924	Unrated 1
731	924	Unrated 2
732	924	Unrated 3
733	924	Unrated 4
734	924	Unrated 5
735	924	Unrated 6
736	924	Unrated 7
737	924	Unrated 8
738	924	Unrated 9
739	806	Headquarters
740	806	Briefing Room
741	806	The Bunker
742	806	Mess Hall
743	946	Room 1
744	946	Room 2
745	922	Competitive
746	922	Friendly
747	918	US - Eastern
748	918	US - Central
749	918	US - Western
750	918	Europe - English
751	918	Europe - French
752	918	Europe - Italian
753	918	Europe - German
754	918	Europe - Spanish
755	976	Beginners
756	976	Intermediate
757	908	Beginners
758	908	Experts
759	908	Europe
760	908	America
761	908	Asia
766	960	Casual Play
767	960	Rated Play
768	960	Can of Spam
769	1008	Public Demo League
770	1008	Public Demo League
771	1008	Public Demo League
776	1004	Amateur
786	1004	Rookie
796	1004	Pro
812	1004	Legend
816	946	Room 3
817	1030	Beginner
818	1030	Intermediate
819	1030	Expert
820	878	News and Events#1
821	878	News and Events#2
822	878	News and Events#3
823	878	News and Events#4
824	878	News and Events#5
830	878	Medal of Honor Chat#1
831	878	Medal of Honor Chat#2
832	878	Medal of Honor Chat#3
833	878	Medal of Honor Chat#4
834	878	Medal of Honor Chat#5
840	878	EA Chat#1
841	878	EA Chat#2
850	878	Tech Support & Help#1
851	878	Tech Support & Help#2
860	878	Clan Arena#1
861	878	Clan Arena#2
862	878	Clan Arena#3
863	878	Clan Arena#4
864	878	Clan Arena#5
865	878	Clan Arena#6
866	878	Clan Arena#7
867	878	Clan Arena#8
868	878	Clan Arena#9
869	878	Clan Arena#10
870	878	Boot Camp Training#1
871	878	Boot Camp Training#2
872	878	Boot Camp Training#3
873	878	Boot Camp Training#4
874	878	Boot Camp Training#5
880	878	Officers Club#1
881	878	Officers Club#2
882	878	Officers Club#3
883	878	Officers Club#4
886	878	Officers Club#5
890	878	The War Room#1
891	878	The War Room#2
892	878	The War Room#3
893	878	The War Room#4
894	878	The War Room#5
900	878	Off Topic Discussion#1
901	878	Off Topic Discussion#2
902	878	Off Topic Discussion#3
903	878	Off Topic Discussion#4
904	878	Off Topic Discussion#5
916	1043	Amateur
925	1043	Rookie
935	1043	Pro
951	1043	Legend
955	1042	Conquest
956	1042	King of the Hill
957	1042	Territory Control
960	843	Reliance WebWorld Tournament
963	946	Room 4
964	946	Room 5
965	946	Room 6
966	946	Room 7
967	946	Room 8
968	946	Room 9
969	946	Room 10
970	1064	Main
971	1064	Tournament
972	955	Airliners
973	955	Adventures
974	955	Bush Flying
975	955	Competitions
976	955	Flight Training
977	955	Fly-Ins
978	955	Free Flight
979	955	Helicopter Ops
983	827	AoX chat
984	827	AoX AUS
985	827	AoX CAN
986	827	AoX CHN
987	827	AoX CZE
988	827	AoX DEU
989	827	AoX ESP
990	827	AoX FRA
991	827	AoX GBR
992	827	AoX HUN
993	827	AoX ITA
994	827	AoX KOR
995	827	AoX POL
996	827	AoX RUS
997	827	AoX USA
998	827	AoX AFRICA
999	827	AoX AMERICA
1000	827	AoX ASIA
1001	827	AoX EUROPE
1002	870	LobbyRoom3
1003	870	LobbyRoom4
1004	870	LobbyRoom5
1005	870	LobbyRoom6
1006	870	LobbyRoom7
1007	870	LobbyRoom8
1008	870	LobbyRoom9
1009	870	ChatRoom1
1010	870	ChatRoom2
1011	870	ChatRoom3
1012	870	ChatRoom4
1013	870	ChatRoom5
1014	870	ChatRoom6
1015	870	ChatRoom7
1016	870	ChatRoom8
1017	870	ChatRoom9
1018	870	ChatRoom10
1019	1070	Amateur 01
1020	1070	Amateur 02
1021	1070	Amateur 03
1022	1070	Amateur 04
1023	1070	Amateur 05
1029	1070	Rookie 01
1030	1070	Rookie 02
1031	1070	Rookie 03
1032	1070	Rookie 04
1033	1070	Rookie 05
1034	1070	Pro 01
1035	1070	Pro 02
1036	1070	Pro 03
1037	1070	Pro 04
1038	1070	Pro 05
1049	1070	Legend 01
1050	1070	Legend 02
1051	1070	Legend 03
1052	1070	Legend 04
1053	1070	Legend 05
1057	1071	Amateur 01
1058	1071	Amateur 02
1059	1071	Amateur 03
1060	1071	Amateur 04
1061	1071	Amateur 05
1062	1071	Amateur 06
1063	1071	Amateur 07
1064	1071	Amateur 08
1065	1071	Amateur 09
1066	1071	Amateur 10
1067	1071	Rookie 01
1068	1071	Rookie 02
1069	1071	Rookie 03
1070	1071	Rookie 04
1071	1071	Rookie 05
1072	1071	Rookie 06
1073	1071	Rookie 07
1074	1071	Rookie 08
1075	1071	Rookie 09
1076	1071	Rookie 10
1077	1071	Pro 01
1078	1071	Pro 02
1079	1071	Pro 03
1080	1071	Pro 04
1081	1071	Pro 05
1082	1071	Pro 06
1083	1071	Pro 07
1084	1071	Pro 08
1085	1071	Pro 09
1086	1071	Pro 10
1087	1071	Pro 11
1088	1071	Pro 12
1089	1071	Pro 13
1090	1071	Pro 14
1091	1071	Pro 15
1092	1071	Pro 16
1093	1071	Legend 01
1094	1071	Legend 02
1095	1079	Amateur 01
1096	1079	Amateur 02
1097	1079	Amateur 03
1098	1079	Amateur 04
1099	1079	Amateur 05
1100	1079	Amateur 06
1105	1079	Rookie 01
1106	1079	Rookie 02
1107	1079	Rookie 03
1108	1079	Rookie 04
1109	1079	Rookie 05
1110	1079	Rookie 06
1115	1079	Pro 01
1116	1079	Pro 02
1117	1079	Pro 03
1118	1079	Pro 04
1119	1079	Pro 05
1120	1079	Pro 06
1131	1079	Legend 01
1132	1079	Legend 02
1133	1079	Legend 03
1134	1079	Legend 04
1135	922	Chat Lobby
1136	1080	Main
1137	1080	Tournament
1151	1088	Welcome
1152	1094	AoX chat
1153	1094	AoX AUS
1154	1094	AoX CAN
1155	1094	AoX CHN
1156	1094	AoX CZE
1157	1094	AoX DEU
1158	1094	AoX ESP
1159	1094	AoX FRA
1160	1094	AoX GBR
1161	1094	AoX HUN
1162	1094	AoX ITA
1163	1094	AoX KOR
1164	1094	AoX POL
1165	1094	AoX RUS
1166	1094	AoX USA
1167	1094	AoX AFRICA
1168	1094	AoX AMERICA
1169	1094	AoX ASIA
1170	1094	AoX EUROPE
1171	1071	Legend 03
1172	1071	Legend 04
1173	976	Advanced
1174	1092	Beginner
1175	1092	Intermediate
1176	1092	Advanced
1177	845	Isteru
1178	845	Gregor
1179	845	Gregor
1180	845	Rahvan
1181	845	Rahvan
1182	845	Feandan
1183	845	Feandan
1184	845	Dalziel
1185	845	Dalziel
1186	845	Istaura
1187	845	Istaura
1188	845	Agarrus
1189	845	Agarrus
1190	845	Lorethal
1191	845	Lorethal
1192	845	Vistira
1193	845	Vistira
1194	845	Keh
1195	845	Keh
1196	845	Soranith
1197	845	Soranith
1198	845	Rubicon
1199	845	Rubicon
1200	845	Thena
1201	845	Thena
1202	845	Artech
1203	845	Artech
1204	845	Ethaniel
1205	845	Ethaniel
1206	845	Kale
1207	845	Kale
1208	845	Calix
1209	845	Calix
1210	845	Culahn
1211	845	Culahn
1212	845	Rowain
1213	845	Rowain
1214	845	Kelis Carthok
1215	845	Kelis Carthok
1223	1035	Main
1224	1035	Asia
1225	1035	Europe
1226	1035	US
1227	1035	Main
1228	1035	Asia
1229	1035	Europe
1230	1035	US
1231	1035	Main
1232	1035	Asia
1233	1035	Europe
1234	1035	US
1235	1135	Novices
1236	1135	Veterans
1237	1135	English
1238	1135	French
1239	1135	German
1240	1135	Italian
1241	1135	Spanish
1250	952	US - Eastern
1251	952	US - Central
1253	952	US - Western
1254	952	Europe - English
1255	952	Europe - French
1256	952	Europe - Italian
1257	952	Europe - German
1258	952	Europe - Spanish
1259	1042	Conquest 2
1260	1042	Conquest 3
1261	1156	Main
1262	1156	Main
1263	1156	Main
1264	1042	Territory Control 2
1265	1042	Territory Control 3
1266	1042	King of the Hill 2
1267	1042	King of the Hill 3
1269	1042	Conquest
1270	1042	King of the Hill
1271	1042	Territory Control
1272	1042	Conquest 2
1273	1042	Conquest 3
1274	1042	Territory Control 3
1275	1042	King of the Hill 2
1276	1042	King of the Hill 3
1277	1042	Territory Control 2
1278	1159	West
1279	1159	East
1282	1173	General
1283	1173	OnlineBattle
1284	1181	General
1285	1181	OnlineBattle
1287	1191	Beginner
1288	1191	Intermediate
1289	1191	Advanced
1290	948	North America
1291	948	Europe
1292	948	Asia Pacific
1301	948	Unpatched
1302	1195	Beginner
1303	1195	Intermediate
1304	1195	Advanced
1330	1212	West
1331	1212	East
1334	1190	Generak
1335	1190	OnlineBattle
1336	1213	West
1337	1213	East
1354	1207	Room 1
1355	1207	Room 2
1356	1207	Room 3
1357	1207	Room 4
1358	1207	Room 5
1359	1207	Room 6
1360	1207	Room 7
1361	1207	Room 8
1362	1207	Room 9
1363	1207	Room 10
1369	1	TestGroupRoom
1370	1224	North America
1371	1224	Europe
1372	1224	Asia Pacific
1381	1231	English
1382	1231	French
1383	1231	German
1384	1231	Italian
1385	1231	Spanish
1386	1231	Eastern US
1387	1231	Central US
1388	1231	Western US
1390	1196	Exhibition Single 00
1391	1196	Exhibition Single 01
1392	1196	Exhibition Single 02
1393	1196	Exhibition Single 03
1394	1196	Exhibition Single 04
1395	1196	Exhibition Single 05
1396	1196	Exhibition Single 06
1397	1196	Exhibition Single 07
1398	1196	Exhibition Single 08
1399	1196	Exhibition Single 09
1410	1196	Exhibition Tag 00
1411	1196	Exhibition Tag 01
1412	1196	Exhibition Tag 02
1413	1196	Exhibition Tag 03
1414	1196	Exhibition Tag 04
1415	1196	Exhibition Tag 05
1416	1196	Exhibition Tag 06
1417	1196	Exhibition Tag 07
1418	1196	Exhibition Tag 08
1419	1196	Exhibition Tag 09
1430	1196	Exhibition Main 00
1431	1196	Exhibition Main 01
1432	1196	Exhibition Main 02
1433	1196	Exhibition Main 03
1434	1196	Exhibition Main 04
1435	1196	Exhibition Main 05
1436	1196	Exhibition Main 06
1437	1196	Exhibition Main 07
1438	1196	Exhibition Main 08
1439	1196	Exhibition Main 09
1450	1196	Exhibition Voice 00
1451	1196	Exhibition Voice 01
1452	1196	Exhibition Voice 02
1453	1196	Exhibition Voice 03
1454	1196	Exhibition Voice 04
1455	1196	Exhibition Voice 05
1456	1196	Exhibition Voice 06
1457	1196	Exhibition Voice 07
1458	1196	Exhibition Voice 08
1459	1196	Exhibition Voice 09
1470	1196	TitleMatch Single 00
1471	1196	TitleMatch Single 01
1472	1196	TitleMatch Single 02
1473	1196	TitleMatch Single 03
1474	1196	TitleMatch Single 04
1475	1196	TitleMatch Single 05
1476	1196	TitleMatch Single 06
1477	1196	TitleMatch Single 07
1478	1196	TitleMatch Single 08
1479	1196	TitleMatch Single 09
1490	1196	TitleMatch Tag 00
1491	1196	TitleMatch Tag 01
1492	1196	TitleMatch Tag 02
1493	1196	TitleMatch Tag 03
1494	1196	TitleMatch Tag 04
1495	1196	TitleMatch Tag 05
1496	1196	TitleMatch Tag 06
1497	1196	TitleMatch Tag 07
1498	1196	TitleMatch Tag 08
1499	1196	TitleMatch Tag 09
1510	1196	TitleMatch Main 00
1511	1196	TitleMatch Main 01
1512	1196	TitleMatch Main 02
1513	1196	TitleMatch Main 03
1514	1196	TitleMatch Main 04
1515	1196	TitleMatch Main 05
1516	1196	TitleMatch Main 06
1517	1196	TitleMatch Main 07
1518	1196	TitleMatch Main 08
1519	1196	TitleMatch Main 09
1530	1196	TitleMatch Voice 00
1531	1196	TitleMatch Voice 01
1532	1196	TitleMatch Voice 02
1533	1196	TitleMatch Voice 03
1534	1196	TitleMatch Voice 04
1535	1196	TitleMatch Voice 05
1536	1196	TitleMatch Voice 06
1537	1196	TitleMatch Voice 07
1538	1196	TitleMatch Voice 08
1539	1196	TitleMatch Voice 09
1590	1196	Trade 00
1591	1196	Trade 01
1592	1196	Trade 02
1593	1196	Trade 03
1594	1196	Trade 04
1595	1196	Trade 05
1596	1196	Trade 06
1597	1196	Trade 07
1598	1196	Trade 08
1599	1196	Trade 09
1610	1228	Group Room 1
1611	1228	Group Room 2
1612	1228	Group Room 3
1613	1122	Noodlers
1614	1265	West Coast
1616	1265	East Coast
1620	1266	QuickMatch
1621	1266	ChatRoom2
1622	1266	ChatRoom1
1623	1266	LobbyRoom1
1624	1266	LobbyRoom2
1652	237	First New One
1653	237	Second New One
1654	237	Third New One
1655	237	Fourth New One
1656	1197	Exhibition Single 00
1657	1197	Exhibition Single 01
1658	1197	Exhibition Single 02
1659	1197	Exhibition Single 03
1660	1197	Exhibition Single 04
1661	1197	Exhibition Single 05
1662	1197	Exhibition Single 06
1663	1197	Exhibition Single 07
1664	1197	Exhibition Single 08
1665	1197	Exhibition Single 09
1666	1197	Exhibition Tag 00
1667	1197	Exhibition Tag 01
1668	1197	Exhibition Tag 02
1669	1197	Exhibition Tag 03
1670	1197	Exhibition Tag 04
1671	1197	Exhibition Tag 05
1672	1197	Exhibition Tag 06
1673	1197	Exhibition Tag 07
1674	1197	Exhibition Tag 08
1675	1197	Exhibition Tag 09
1676	1197	Exhibition Main 00
1677	1197	Exhibition Main 01
1678	1197	Exhibition Main 02
1679	1197	Exhibition Main 03
1680	1197	Exhibition Main 04
1681	1197	Exhibition Main 05
1682	1197	Exhibition Main 06
1683	1197	Exhibition Main 07
1684	1197	Exhibition Main 08
1685	1197	Exhibition Main 09
1686	1197	Exhibition Voice 00
1687	1197	Exhibition Voice 01
1688	1197	Exhibition Voice 02
1689	1197	Exhibition Voice 03
1690	1197	Exhibition Voice 04
1691	1197	Exhibition Voice 05
1692	1197	Exhibition Voice 06
1693	1197	Exhibition Voice 07
1694	1197	Exhibition Voice 08
1695	1197	Exhibition Voice 09
1696	1197	TitleMatch Single 00
1697	1197	TitleMatch Single 01
1698	1197	TitleMatch Single 02
1699	1197	TitleMatch Single 03
1700	1197	TitleMatch Single 04
1701	1197	TitleMatch Single 05
1702	1197	TitleMatch Single 06
1703	1197	TitleMatch Single 07
1704	1197	TitleMatch Single 08
1705	1197	TitleMatch Single 09
1706	1197	TitleMatch Tag 00
1707	1197	TitleMatch Tag 01
1708	1197	TitleMatch Tag 02
1709	1197	TitleMatch Tag 03
1710	1197	TitleMatch Tag 04
1711	1197	TitleMatch Tag 05
1712	1197	TitleMatch Tag 06
1713	1197	TitleMatch Tag 07
1714	1197	TitleMatch Tag 08
1715	1197	TitleMatch Tag 09
1716	1197	TitleMatch Main 00
1717	1197	TitleMatch Main 01
1718	1197	TitleMatch Main 02
1719	1197	TitleMatch Main 03
1720	1197	TitleMatch Main 04
1721	1197	TitleMatch Main 05
1722	1197	TitleMatch Main 06
1723	1197	TitleMatch Main 07
1724	1197	TitleMatch Main 08
1725	1197	TitleMatch Main 09
1726	1197	TitleMatch Voice 00
1727	1197	TitleMatch Voice 01
1728	1197	TitleMatch Voice 02
1729	1197	TitleMatch Voice 03
1730	1197	TitleMatch Voice 04
1731	1197	TitleMatch Voice 05
1732	1197	TitleMatch Voice 06
1733	1197	TitleMatch Voice 07
1734	1197	TitleMatch Voice 08
1735	1197	TitleMatch Voice 09
1736	1197	Trade 00
1737	1197	Trade 01
1738	1197	Trade 02
1739	1197	Trade 03
1740	1197	Trade 04
1741	1197	Trade 05
1742	1197	Trade 06
1743	1197	Trade 07
1744	1197	Trade 08
1745	1197	Trade 09
1746	1198	Exhibition Single 00
1747	1198	Exhibition Single 01
1748	1198	Exhibition Single 02
1749	1198	Exhibition Single 03
1750	1198	Exhibition Single 04
1751	1198	Exhibition Tag 00
1752	1198	Exhibition Tag 01
1753	1198	Exhibition Tag 02
1754	1198	Exhibition Tag 03
1755	1198	Exhibition Tag 04
1756	1198	Exhibition Main 00
1757	1198	Exhibition Main 01
1758	1198	Exhibition Main 02
1759	1198	Exhibition Main 03
1760	1198	Exhibition Main 04
1761	1198	Exhibition Voice 00
1762	1198	Exhibition Voice 01
1763	1198	Exhibition Voice 02
1764	1198	Exhibition Voice 03
1765	1198	Exhibition Voice 04
1766	1198	TitleMatch Single 00
1767	1198	TitleMatch Single 01
1768	1198	TitleMatch Single 02
1769	1198	TitleMatch Single 03
1770	1198	TitleMatch Single 04
1771	1198	TitleMatch Tag 00
1772	1198	TitleMatch Tag 01
1773	1198	TitleMatch Tag 02
1774	1198	TitleMatch Tag 03
1775	1198	TitleMatch Tag 04
1776	1198	TitleMatch Main 00
1777	1198	TitleMatch Main 01
1778	1198	TitleMatch Main 02
1779	1198	TitleMatch Main 03
1780	1198	TitleMatch Main 04
1781	1198	TitleMatch Voice 00
1782	1198	TitleMatch Voice 01
1783	1198	TitleMatch Voice 02
1784	1198	TitleMatch Voice 03
1785	1198	TitleMatch Voice 04
1786	1198	Trade 00
1787	1198	Trade 01
1788	1198	Trade 02
1789	1198	Trade 03
1790	1198	Trade 04
1793	1122	Jammers
1795	1066	Rebellion
1796	1066	Empire
1797	1066	Versus (1vs1)
1798	1066	Team Games
1799	1066	General
1800	1066	Rebellion
1801	1066	Empire
1802	1066	Versus (1vs1)
1803	1066	Team Games
1804	1066	General
1805	1066	Rebellion
1806	1066	Empire
1807	1066	Versus (1vs1)
1808	1066	Team Games
1809	1066	General
1810	1066	Rebellion
1811	1066	Empire
1812	1066	Versus (1vs1)
1813	1066	Team Games
1814	1066	General
1815	1066	Rebellion
1816	1066	Empire
1817	1066	Versus (1vs1)
1818	1066	Team Games
1819	1066	General
1820	1066	Rebellion
1821	1066	Empire
1822	1066	Versus (1vs1)
1823	1066	Team Games
1824	1066	General
1825	1066	Rebellion
1826	1066	Empire
1827	1066	Versus (1vs1)
1828	1066	Team Games
1829	1066	General
1830	1066	Rebellion
1831	1066	Empire
1837	1309	West
1838	1309	East
1839	1321	QuickMatch
1840	1321	ChatRoom1
1842	1321	LobbyRoom1
1843	1321	LobbyRoom2
1844	1321	LobbyRoom3
1845	1321	LobbyRoom4
1846	1321	LobbyRoom5
1847	1321	LobbyRoom6
1848	1321	LobbyRoom7
1849	1321	LobbyRoom8
1850	1283	Western US
1851	1283	Central US
1852	1283	Eastern US
1853	1283	German
1854	1283	Spanish
1855	1283	English
1856	1283	Italian
1857	1283	French
1858	1321	LobbyRoom9
1859	1122	Chop Masters
1860	1160	USA
1861	1160	Europe
1862	1190	Debug
1863	1354	Larry
1864	1354	Curly
1865	1354	Moe
1866	1376	West
1867	1376	East
1868	1391	Action
1869	1391	Story
1870	1391	Story Lite
1871	1391	Role Play
1872	1391	Team
1873	1391	Melee
1874	1391	Arena
1875	1391	Social
1876	1391	Alternative
1877	1391	PW Story
1878	1391	PW Action
1879	1391	Solo
1880	1391	Tech Support
1881	1122	eJamming Test
1882	1401	Novices
1883	1401	Veterans
1884	1401	English
1885	1401	French
1886	1401	German
1887	1401	Italian
1888	1401	Spanish
1889	1396	Africa
1890	1396	America
1891	1396	Asia
1892	1396	Europe
1893	1396	Pacific
1894	1396	The Empire
1895	1396	Hordes of Chaos
1896	1396	High Elves
1897	1396	Skaven
1898	1396	Other
1899	1396	Kicked
1900	1396	Banned
1901	1422	QuickMatch
1902	1422	ChatRoom1
1904	1426	Beginner
1905	1426	Intermediate
1906	1426	Advanced
1907	1411	Room 1
1908	1411	Room 2
1909	1411	Room 3
1910	1411	Room 4
1911	1411	Room 5
1912	1411	Room 6
1913	1411	Room 7
1914	1411	Room 8
1915	1411	Room 9
1916	1411	Room 10
1917	1463	QuickMatch
1918	1463	ChatRoom1
1919	1463	LobbyRoom1
1920	1463	LobbyRoom2
1921	1463	LobbyRoom3
1922	1463	LobbyRoom4
1923	1463	LobbyRoom5
1924	1463	LobbyRoom6
1925	1463	LobbyRoom7
1926	1463	LobbyRoom8
1927	1463	LobbyRoom9
1928	1398	Africa
1929	1398	America
1930	1398	Asia
1931	1398	Europe
1932	1398	Pacific
1933	1398	The Empire
1934	1398	Hordes of Chaos
1935	1398	High Elves
1936	1398	Skaven
1937	1398	Other
1938	1398	Kicked
1939	1398	Banned
1940	1479	West
1941	1479	East
1942	1481	East
1943	1481	West
1944	1463	QuickMatch2
1945	1483	America
1946	1483	Asia
1947	1483	Europe
1948	1321	QuickMatch2
1956	1514	Newbies
1957	1514	Experienced Players
1958	1514	Strategists
1959	1515	Newbies
1960	1515	Experienced Players
1961	1515	Strategists
1962	1523	Skirmish
1963	1523	World Domination
1964	1524	Skirmish
1965	1524	World Domination
1966	1577	Rebellion
1967	1577	Empire
1968	1577	Versus (1vs1)
1969	1577	Team Games
1970	1577	General
1971	1577	Rebellion
1972	1577	Empire
1973	1577	Versus (1vs1)
1974	1577	Team Games
1975	1577	General
1976	1577	Rebellion
1977	1577	Empire
1978	1577	Versus (1vs1)
1979	1577	Team Games
1980	1577	General
1981	1577	Rebellion
1982	1577	Empire
1983	1577	Versus (1vs1)
1984	1577	Team Games
1985	1577	General
1986	1577	Rebellion
1987	1577	Empire
1988	1577	Versus (1vs1)
1989	1577	Team Games
1990	1577	General
1991	1577	Rebellion
1992	1577	Empire
1993	1577	Versus (1vs1)
1994	1577	Team Games
1995	1577	General
1996	1577	Rebellion
1997	1577	Empire
1998	1577	Versus (1vs1)
1999	1577	Team Games
2000	1577	General
2001	1577	Rebellion
2002	1577	Empire
2003	1631	Skirmish
2004	1631	World Domination
2027	1714	ChatRoom1
2028	1714	QuickMatch
2029	1714	LobbyRoom:1
2030	1714	LobbyRoom:2
2031	1714	LobbyRoom:3
2032	1714	LobbyRoom:4
2033	1714	LobbyRoom:5
2034	1714	LobbyRoom:6
2035	1714	LobbyRoom:7
2036	1714	LobbyRoom:8
2037	1714	LobbyRoom:9
2038	1714	LobbyRoom:10
2039	1714	LobbyRoom:11
2040	1714	LobbyRoom:12
2041	1714	LobbyRoom:13
2042	1714	LobbyRoom:14
2043	1714	LobbyRoom:15
2044	1714	LobbyRoom:16
2045	1714	LobbyBeginners:1
2046	1714	LobbyHardcore:1
2047	1714	LobbyClan:1
2048	1714	LobbyClan:2
2049	1714	LobbyTournaments:1
2050	1714	LobbyTournaments:2
2051	1714	LobbyBattlecast:1
2052	1714	LobbyCustomMap:1
2053	1714	LobbyCustomMap:2
2054	1714	LobbyCompStomp:1
2055	1714	LobbyGerman:1
2056	1714	LobbyGerman:2
2057	1714	LobbyKorean:1
2058	1714	LobbyFrench:1
2059	1422	LobbyRoom:1
2060	1422	LobbyRoom:2
2061	1422	LobbyRoom:3
2062	1422	LobbyRoom:4
2063	1422	LobbyRoom:5
2064	1422	LobbyRoom:6
2065	1422	LobbyRoom:7
2066	1422	LobbyRoom:8
2067	1422	LobbyRoom:9
2068	1422	LobbyRoom:10
2069	1422	LobbyRoom:11
2070	1422	LobbyRoom:12
2071	1422	LobbyRoom:13
2072	1422	LobbyRoom:14
2073	1422	LobbyRoom:15
2074	1422	LobbyRoom:16
2075	1422	LobbyBeginners:1
2076	1422	LobbyHardcore:1
2077	1422	LobbyClan:1
2078	1422	LobbyClan:2
2079	1422	LobbyTournaments:1
2080	1422	LobbyTournaments:2
2081	1422	LobbyBattlecast:1
2082	1422	LobbyCustomMap:1
2083	1422	LobbyCustomMap:2
2084	1422	LobbyCompStomp:1
2085	1422	LobbyGerman:1
2086	1422	LobbyGerman:2
2087	1422	LobbyKorean:1
2088	1422	LobbyFrench:1
2089	1774	Room 1
2090	1774	Room 2
2091	1774	Room 3
2092	1774	Room 4
2093	1774	Room 5
2094	1774	Room 6
2095	1774	Room 7
2096	1774	Room 8
2097	1774	Room 9
2098	1774	Room 10
2099	1814	ChatRoom1
2100	1814	QuickMatch
2101	1814	LobbyRoom:1
2102	1814	LobbyRoom:2
2103	1814	LobbyRoom:3
2104	1814	LobbyRoom:4
2105	1814	LobbyRoom:5
2106	1814	LobbyRoom:6
2107	1814	LobbyRoom:7
2108	1814	LobbyRoom:8
2109	1814	LobbyRoom:9
2110	1814	LobbyRoom:10
2111	1814	LobbyRoom:11
2112	1814	LobbyRoom:12
2113	1814	LobbyRoom:13
2114	1814	LobbyRoom:14
2115	1814	LobbyRoom:15
2116	1814	LobbyRoom:16
2117	1814	LobbyBeginners:1
2118	1814	LobbyHardcore:1
2119	1814	LobbyClan:1
2120	1814	LobbyClan:2
2121	1814	LobbyTournaments:1
2122	1814	LobbyTournaments:2
2123	1814	LobbyBattlecast:1
2124	1814	LobbyCustomMap:1
2125	1814	LobbyCustomMap:2
2126	1814	LobbyCompStomp:1
2127	1814	LobbyGerman:1
2128	1814	LobbyGerman:2
2129	1814	LobbyKorean:1
2130	1814	LobbyFrench:1
2131	1884	Room 1
2132	1884	Room 2
2133	1884	Room 3
2134	1884	Room 4
2135	1884	Room 5
2136	1884	Room 6
2137	1884	Room 7
2138	1884	Room 8
2139	1884	Room 9
2140	1884	Room 10
2141	1923	Africa
2142	1923	America
2143	1923	Asia
2144	1923	Europe
2145	1923	Pacific
2146	1923	The Empire
2147	1923	Hordes of Chaos
2148	1923	High Elves
2149	1923	Skaven
2150	1923	Other
2151	1923	Dark Elf
2152	1923	Orc
2153	1923	Kicked
2154	1923	Banned
2155	1514	Sparta FX
2156	1979	QuickMatch
2157	1979	LobbyRoom:1
2158	1979	ChatRoom1
2159	2100	QuickMatch
2160	2100	LobbyRoom:1
2161	2100	ChatRoom1
2162	2094	QuickMatch
2163	2094	LobbyRoom:1
2164	2094	ChatRoom1
2166	2128	LobbyRoom:1
2167	2128	LobbyRoom:2
2168	2128	LobbyRoom:3
2169	2128	LobbyRoom:4
2170	2128	LobbyRoom:5
2171	2128	LobbyKorean:1
2172	2128	LobbyFrench:1
2173	2128	LobbyGerman:2
2174	2128	LobbyGerman:1
2175	2128	LobbyBattlecast:1
2176	2128	LobbyRoom:6
2177	2128	ChatRoom1
2178	2128	LobbyRoom:8
2179	2128	LobbyRoom:11
2180	2128	LobbyRoom:7
2181	2128	LobbyRoom:9
2182	2128	LobbyRoom:12
2183	2128	LobbyRoom:10
2184	2128	LobbyClan:1
2185	2128	LobbyRoom:13
2186	2128	LobbyClan:2
2187	2128	LobbyRoom:14
2188	2128	LobbyRoom:16
2189	2128	LobbyRoom:15
2190	2128	LobbyBeginners:1
2191	2128	LobbyTournaments:1
2192	2128	LobbyCompStomp:1
2193	2128	LobbyHardcore:1
2194	2128	LobbyCustomMap:1
2195	2128	LobbyCustomMap:2
2196	2128	LobbyTournaments:2
2198	2128	ChatRoom1
2200	2130	LobbyRoom:1
2201	2130	LobbyRoom:2
2202	2130	LobbyRoom:3
2203	2130	LobbyRoom:4
2204	2130	LobbyRoom:5
2205	2130	LobbyKorean:1
2206	2130	LobbyFrench:1
2207	2130	LobbyGerman:2
2208	2130	LobbyGerman:1
2209	2130	LobbyBattlecast:1
2210	2130	LobbyRoom:6
2211	2130	ChatRoom1
2212	2130	LobbyRoom:8
2213	2130	LobbyRoom:11
2214	2130	LobbyRoom:7
2215	2130	LobbyRoom:9
2216	2130	LobbyRoom:12
2217	2130	LobbyRoom:10
2218	2130	LobbyClan:1
2219	2130	LobbyRoom:13
2220	2130	LobbyClan:2
2221	2130	LobbyRoom:14
2222	2130	LobbyRoom:16
2223	2130	LobbyRoom:15
2224	2130	LobbyBeginners:1
2225	2130	LobbyTournaments:1
2226	2130	LobbyCompStomp:1
2227	2130	LobbyHardcore:1
2228	2130	LobbyCustomMap:1
2229	2130	LobbyCustomMap:2
2230	2130	LobbyTournaments:2
2232	2130	ChatRoom1
2233	2160	QuickMatch
2234	2160	LobbyTournaments:2
2235	2160	LobbyTournaments:1
2236	2160	LobbyRoom:9
2237	2160	LobbyRoom:8
2238	2160	LobbyRoom:7
2239	2160	LobbyRoom:6
2240	2160	LobbyRoom:5
2241	2160	LobbyRoom:4
2242	2160	LobbyRoom:3
2243	2160	LobbyRoom:2
2244	2160	LobbyRoom:16
2245	2160	LobbyRoom:15
2246	2160	LobbyRoom:14
2247	2160	LobbyRoom:13
2248	2160	LobbyRoom:12
2249	2160	LobbyRoom:11
2250	2160	LobbyRoom:10
2251	2160	LobbyRoom:1
2252	2160	LobbyKorean:1
2253	2160	LobbyHardcore:1
2254	2160	LobbyGerman:2
2255	2160	LobbyGerman:1
2256	2160	LobbyFrench:1
2257	2160	LobbyCustomMap:2
2258	2160	LobbyCustomMap:1
2259	2160	LobbyCompStomp:1
2260	2160	LobbyClan:2
2261	2160	LobbyClan:1
2262	2160	LobbyBeginners:1
2263	2160	LobbyBattlecast:1
2264	2160	ChatRoom1
2265	1979	Russia:1
2266	2238	LobbyRoom:1
2267	2238	LobbyRoom:2
2268	2238	LobbyRoom:3
2269	2238	LobbyRoom:4
2270	2238	LobbyRoom:5
2271	2238	LobbyRoom:6
2272	2238	LobbyRoom:7
2273	2238	LobbyRoom:8
2274	2238	LobbyRoom:9
2275	2238	LobbyRoom:10
2276	2238	LobbyRoom:11
2277	2238	LobbyRoom:12
2278	2238	LobbyRoom:13
2279	2238	LobbyRoom:14
2280	2238	LobbyRoom:15
2281	2238	LobbyRoom:16
2282	2238	LobbyBeginners:1
2283	2238	LobbyHardcore:1
2284	2238	LobbyClan:1
2285	2238	LobbyClan:2
2286	2238	LobbyTournaments:1
2287	2238	LobbyTournaments:2
2288	2238	LobbyBattlecast:1
2289	2238	LobbyCustomMap:1
2290	2238	LobbyCustomMap:2
2291	2238	LobbyCompStomp:1
2292	2238	LobbyGerman:1
2293	2238	LobbyGerman:2
2294	2238	LobbyKorean:1
2295	2238	LobbyFrench:1
2296	2238	ChatRoom1
2297	1814	Russia:1
2298	2298	Deathmatch
2299	2298	RopeRace
2300	2298	Forts
2301	2298	Triathlon
2302	2313	Beginner
2303	2313	Intermediate
2304	2313	Expert
2305	2374	ChatRoom1
2306	2374	LobbyRoom:1
2307	2374	LobbyRoom:2
2308	2374	LobbyRoom:3
2309	2374	LobbyRoom:4
2310	2374	LobbyRoom:5
2311	2374	LobbyKorean:1
2312	2374	LobbyFrench:1
2313	2374	LobbyGerman:2
2314	2374	LobbyGerman:1
2315	2374	LobbyBattlecast:1
2316	2374	LobbyRoom:6
2317	2374	ChatRoom1
2318	2374	LobbyRoom:8
2319	2374	LobbyRoom:11
2320	2374	LobbyRoom:7
2321	2374	LobbyRoom:9
2322	2374	LobbyRoom:12
2323	2374	LobbyRoom:10
2324	2374	LobbyClan:1
2325	2374	LobbyRoom:13
2326	2374	LobbyClan:2
2327	2374	LobbyRoom:14
2328	2374	LobbyRoom:16
2329	2374	LobbyRoom:15
2330	2374	LobbyBeginners:1
2331	2374	LobbyTournaments:1
2332	2374	LobbyCompStomp:1
2333	2374	LobbyHardcore:1
2334	2374	LobbyCustomMap:1
2335	2374	LobbyCustomMap:2
2336	2374	LobbyTournaments:2
2337	2130	LobbyRussian:1
2338	2130	LobbyTaiwan:1
2339	2128	LobbyRoom:17
2340	2128	LobbyRoom:18
2341	2128	LobbyRoom:19
2342	2128	LobbyRoom:20
2343	2128	LobbyRoom:21
2344	2128	LobbyCoop:1
2345	2128	LobbyCoop:2
2346	2128	LobbyCoop:3
2347	2128	LobbyCoop:4
2348	2128	LobbyCoop:5
2349	2128	LobbyRussian:1
2350	2128	LobbyTaiwan:1
2351	2298	WarRoom
2352	2298	TacticsMode
2353	2259	room1
2354	2259	room2
2355	2259	room3
2356	2259	room4
2357	2128	LobbySpanish:1
2358	2714	room1
2359	2714	room2
2360	2714	room3
2361	2714	room4
2362	2246	ChatMonTesting
2363	2712	room1
2364	2712	room2
2365	2712	room3
2366	2712	room4
2367	2705	North America
2368	2705	Europe
2369	2705	Brazil
2370	2840	FX Sparta II
2371	2706	North America
2372	2706	Europe
2373	2706	Brazil
2374	2707	North America
2375	2707	Europe
2376	2707	Brazil
2377	2032	North America
2378	2032	Europe
2379	2032	Brazil
2380	2034	North America
2381	2034	Europe
2382	2034	Brazil
2383	2035	North America
2384	2035	Europe
2385	2035	Brazil
2386	706	Main Lobby
2387	1003	ThMods.com
2388	917	Main Lobby
2389	1128	Main Lobby
2390	1307	ThMods.com
2391	1005	ThMods.com
2392	467	Main Lobby
2393	600	Main Lobby
2399	1003	Lobby 2
\.


--
-- TOC entry 3448 (class 0 OID 16410)
-- Dependencies: 217
-- Data for Name: messages; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.messages (messageid, namespaceid, type, "from", "to", date, message) FROM stdin;
\.


--
-- TOC entry 3450 (class 0 OID 16417)
-- Dependencies: 219
-- Data for Name: partner; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.partner (partnerid, partnername) FROM stdin;
0	UniSpy
95	Crytek
\.


--
-- TOC entry 3451 (class 0 OID 16422)
-- Dependencies: 220
-- Data for Name: profiles; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.profiles (profileid, userid, nick, serverflag, status, statstring, location, firstname, lastname, publicmask, latitude, longitude, aim, picture, occupationid, incomeid, industryid, marriedid, childcount, interests1, ownership1, connectiontype, sex, zipcode, countrycode, homepage, birthday, birthmonth, birthyear, icquin, quietflags, streetaddr, streeaddr, city, cpubrandid, cpuspeed, memory, videocard1string, videocard1ram, videocard2string, videocard2ram, subscription, adminrights) FROM stdin;
1	1	spyguy	0	0	I love UniSpy	earth	spy	guy	0	0	0		0	0	0	0	0	0	0	0	0	0	00000	1	unispy.org	0	0	0	0	0	\N	\N	\N	0	0	0	\N	0	\N	0	0	0
2	2	uniguy	0	0	I love UniSpy	earth	uni	guy	0	0	0		0	0	0	0	0	0	0	0	0	0	00000	1	unispy.org	0	0	0	0	0	\N	\N	\N	0	0	0	\N	0	\N	0	0	0
3	3	gptest1	0	0	I love UniSpy	\N	\N	\N	0	0	0		0	0	0	0	0	0	0	0	0	0	00000	1	unispy.org	0	0	0	0	0	\N	\N	\N	0	0	0	\N	0	\N	0	0	0
4	4	gptest2	0	0	I love UniSpy	\N	\N	\N	0	0	0		0	0	0	0	0	0	0	0	0	0	00000	1	unispy.org	0	0	0	0	0	\N	\N	\N	0	0	0	\N	0	\N	0	0	0
5	5	gptest3	0	0	I love UniSpy	\N	\N	\N	0	0	0		0	0	0	0	0	0	0	0	0	0	00000	1	unispy.org	0	0	0	0	0	\N	\N	\N	0	0	0	\N	0	\N	0	0	0
\.


--
-- TOC entry 3453 (class 0 OID 16460)
-- Dependencies: 222
-- Data for Name: pstorage; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.pstorage (pstorageid, profileid, ptype, dindex, data) FROM stdin;
1	1	1	0	\N
3	1	3	0	\N
\.


--
-- TOC entry 3455 (class 0 OID 16466)
-- Dependencies: 224
-- Data for Name: sakestorage; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.sakestorage (sakestorageid, tableid) FROM stdin;
\.


--
-- TOC entry 3457 (class 0 OID 16472)
-- Dependencies: 226
-- Data for Name: subprofiles; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.subprofiles (subprofileid, profileid, uniquenick, namespaceid, partnerid, productid, gamename, cdkeyenc, firewall, port, authtoken) FROM stdin;
1	1	spyguy	1	0	0	gmtest	\N	0	0	example_token
2	2	uniguy	1	0	0	gmtest	\N	0	0	\N
3	3	gptest1	1	0	0	gmtest	\N	0	0	\N
4	4	gptest2	1	0	0	gmtest	\N	0	0	\N
5	5	gptest3	1	0	0	gmtest	\N	0	0	\N
\.


--
-- TOC entry 3459 (class 0 OID 16482)
-- Dependencies: 228
-- Data for Name: users; Type: TABLE DATA; Schema: unispy; Owner: -
--

COPY unispy.users (userid, email, password, emailverified, lastip, lastonline, createddate, banned, deleted) FROM stdin;
1	spyguy@gamespy.com	4a7d1ed414474e4033ac29ccb8653d9b	t	\N	2022-01-19 20:01:49.828006	2022-01-19 20:01:49.828006	f	f
2	uni@unispy.org	4a7d1ed414474e4033ac29ccb8653d9b	t	\N	2022-01-19 20:02:57.595514	2022-01-19 20:02:57.595514	f	f
3	gptestc1@gptestc.com	c6d525669e64438c9aa156a0cc80cd14	t	\N	2022-01-19 20:03:44.754069	2022-01-19 20:03:44.754069	f	f
4	gptestc2@gptestc.com	c6d525669e64438c9aa156a0cc80cd14	t	\N	2022-01-19 20:03:44.761986	2022-01-19 20:03:44.761986	f	f
5	gptestc3@gptestc.com	c6d525669e64438c9aa156a0cc80cd14	t	\N	2022-01-19 20:03:44.764527	2022-01-19 20:03:44.764527	f	f
\.


--
-- TOC entry 3488 (class 0 OID 0)
-- Dependencies: 210
-- Name: addrequests_addrequestid_seq; Type: SEQUENCE SET; Schema: unispy; Owner: -
--

SELECT pg_catalog.setval('unispy.addrequests_addrequestid_seq', 1, false);


--
-- TOC entry 3489 (class 0 OID 0)
-- Dependencies: 212
-- Name: blocked_blockid_seq; Type: SEQUENCE SET; Schema: unispy; Owner: -
--

SELECT pg_catalog.setval('unispy.blocked_blockid_seq', 1, false);


--
-- TOC entry 3490 (class 0 OID 0)
-- Dependencies: 214
-- Name: friends_friendid_seq; Type: SEQUENCE SET; Schema: unispy; Owner: -
--

SELECT pg_catalog.setval('unispy.friends_friendid_seq', 1, false);


--
-- TOC entry 3491 (class 0 OID 0)
-- Dependencies: 218
-- Name: messages_messageid_seq; Type: SEQUENCE SET; Schema: unispy; Owner: -
--

SELECT pg_catalog.setval('unispy.messages_messageid_seq', 1, false);


--
-- TOC entry 3492 (class 0 OID 0)
-- Dependencies: 221
-- Name: profiles_profileid_seq; Type: SEQUENCE SET; Schema: unispy; Owner: -
--

SELECT pg_catalog.setval('unispy.profiles_profileid_seq', 6, true);


--
-- TOC entry 3493 (class 0 OID 0)
-- Dependencies: 223
-- Name: pstorage_pstorageid_seq; Type: SEQUENCE SET; Schema: unispy; Owner: -
--

SELECT pg_catalog.setval('unispy.pstorage_pstorageid_seq', 3, true);


--
-- TOC entry 3494 (class 0 OID 0)
-- Dependencies: 225
-- Name: sakestorage_sakestorageid_seq; Type: SEQUENCE SET; Schema: unispy; Owner: -
--

SELECT pg_catalog.setval('unispy.sakestorage_sakestorageid_seq', 1, false);


--
-- TOC entry 3495 (class 0 OID 0)
-- Dependencies: 227
-- Name: subprofiles_subprofileid_seq; Type: SEQUENCE SET; Schema: unispy; Owner: -
--

SELECT pg_catalog.setval('unispy.subprofiles_subprofileid_seq', 5, true);


--
-- TOC entry 3496 (class 0 OID 0)
-- Dependencies: 229
-- Name: users_userid_seq; Type: SEQUENCE SET; Schema: unispy; Owner: -
--

SELECT pg_catalog.setval('unispy.users_userid_seq', 5, true);


--
-- TOC entry 3271 (class 2606 OID 16503)
-- Name: addrequests addrequests_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.addrequests
    ADD CONSTRAINT addrequests_pk PRIMARY KEY (addrequestid);


--
-- TOC entry 3273 (class 2606 OID 16505)
-- Name: blocked blocked_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.blocked
    ADD CONSTRAINT blocked_pk PRIMARY KEY (blockid);


--
-- TOC entry 3275 (class 2606 OID 16507)
-- Name: friends friends_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.friends
    ADD CONSTRAINT friends_pk PRIMARY KEY (friendid);


--
-- TOC entry 3277 (class 2606 OID 16509)
-- Name: games games_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.games
    ADD CONSTRAINT games_pk PRIMARY KEY (gameid);


--
-- TOC entry 3279 (class 2606 OID 16511)
-- Name: grouplist grouplist_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.grouplist
    ADD CONSTRAINT grouplist_pk PRIMARY KEY (groupid);


--
-- TOC entry 3281 (class 2606 OID 16513)
-- Name: messages messages_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.messages
    ADD CONSTRAINT messages_pk PRIMARY KEY (messageid);


--
-- TOC entry 3283 (class 2606 OID 16515)
-- Name: partner partner_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.partner
    ADD CONSTRAINT partner_pk PRIMARY KEY (partnerid);


--
-- TOC entry 3285 (class 2606 OID 16517)
-- Name: profiles profiles_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.profiles
    ADD CONSTRAINT profiles_pk PRIMARY KEY (profileid);


--
-- TOC entry 3287 (class 2606 OID 16519)
-- Name: pstorage pstorage_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.pstorage
    ADD CONSTRAINT pstorage_pk PRIMARY KEY (pstorageid);


--
-- TOC entry 3289 (class 2606 OID 16521)
-- Name: sakestorage sakestorage_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.sakestorage
    ADD CONSTRAINT sakestorage_pk PRIMARY KEY (sakestorageid);


--
-- TOC entry 3291 (class 2606 OID 16523)
-- Name: subprofiles subprofiles_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.subprofiles
    ADD CONSTRAINT subprofiles_pk PRIMARY KEY (subprofileid);


--
-- TOC entry 3293 (class 2606 OID 16525)
-- Name: users users_pk; Type: CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.users
    ADD CONSTRAINT users_pk PRIMARY KEY (userid);


--
-- TOC entry 3294 (class 2606 OID 16526)
-- Name: addrequests addrequests_fk; Type: FK CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.addrequests
    ADD CONSTRAINT addrequests_fk FOREIGN KEY (profileid) REFERENCES unispy.profiles(profileid);


--
-- TOC entry 3295 (class 2606 OID 16531)
-- Name: blocked blocked_fk; Type: FK CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.blocked
    ADD CONSTRAINT blocked_fk FOREIGN KEY (profileid) REFERENCES unispy.profiles(profileid);


--
-- TOC entry 3296 (class 2606 OID 16536)
-- Name: friends friends_fk; Type: FK CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.friends
    ADD CONSTRAINT friends_fk FOREIGN KEY (profileid) REFERENCES unispy.profiles(profileid);


--
-- TOC entry 3297 (class 2606 OID 16541)
-- Name: grouplist grouplist_fk; Type: FK CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.grouplist
    ADD CONSTRAINT grouplist_fk FOREIGN KEY (gameid) REFERENCES unispy.games(gameid);


--
-- TOC entry 3298 (class 2606 OID 16546)
-- Name: profiles profiles_fk; Type: FK CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.profiles
    ADD CONSTRAINT profiles_fk FOREIGN KEY (userid) REFERENCES unispy.users(userid);


--
-- TOC entry 3299 (class 2606 OID 16551)
-- Name: pstorage pstorage_fk; Type: FK CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.pstorage
    ADD CONSTRAINT pstorage_fk FOREIGN KEY (profileid) REFERENCES unispy.profiles(profileid);


--
-- TOC entry 3300 (class 2606 OID 16556)
-- Name: subprofiles subprofiles_fk; Type: FK CONSTRAINT; Schema: unispy; Owner: -
--

ALTER TABLE ONLY unispy.subprofiles
    ADD CONSTRAINT subprofiles_fk FOREIGN KEY (profileid) REFERENCES unispy.profiles(profileid);


-- Completed on 2022-02-27 02:04:35 CET

--
-- PostgreSQL database dump complete
--

