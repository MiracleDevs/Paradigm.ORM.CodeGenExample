CREATE KEYSPACE IF NOT EXISTS tracking WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor': 3};

use tracking;

CREATE TABLE IF NOT EXISTS tracking_data (
       first_name text,
       last_name text,
       timestamp timestamp,
       location varchar,
       speed double,
       heat double,
       telepathy_powers int,
       primary key((first_name, last_name), timestamp))
       WITH CLUSTERING ORDER BY (timestamp DESC)
       AND COMPACTION = {'class': 'DateTieredCompactionStrategy',
           'base_time_seconds': 3600,
           'max_sstable_age_days': 1};
