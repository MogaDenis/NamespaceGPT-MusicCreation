trackRepository interface:
- void add(Track elem)
- void delete(Track elem) throws Exception
- Track? search(int id)
- Track[] getAll() <- might suffer modifications

songRepository interface:
- void add(Song elem)
- void delete(Track elem) throws Exception
- Song? search(int id)
- Song[] getAll() <- might suffer modifications

musictagRepository interface:
- void add(MusicTag elem)
- MusicTag? search(int id)
- MusicTag[] getAll() <- might suffer modifications

songBuilder interface:
- *should have an internal list*
- void add(Playable elem)
- void remove(Playable or int)
- Playable getList()

Service interface:
- void addTrack(Track elem)
- void deleteTrack(Track elem)
- Track? searchTrack(int id)
- Track[] getAllTracks()
- private void addSong(Song elem)
- void deleteSong(Song elem)
- Song? searchSong(int id)
- Song[] getAllSongs()
- void add(MusicTag elem)
- MusicTag? searchTag(int id)
- MusicTag[] getAllTags()
- Song create(Track[] elements)

*Playable* file format: .wav


#### How to do Git:
- Create branch on GitHub
- Pull on local repository
- Checkout local branch and work on it
- Stage and commit as often as possible, and push when done
- Create pull request on GitHub
 
For more details:\
https://www.freecodecamp.org/news/how-to-use-git-and-github-in-a-team-like-a-pro#github-workflow
