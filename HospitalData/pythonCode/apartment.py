# -*- coding:utf-8 -*-
import re
import urllib2

import MySQLdb

db = MySQLdb.connect("localhost", "root", "******", "guahao", charset="utf8")
cursor = db.cursor()


def get_html(url):
    user_agent = 'Mozilla/4.0 (compatible; MSIE 5.5; Windows NT)'
    headers = {'User-Agent': user_agent}
    request = urllib2.Request(url, headers=headers)
    html = urllib2.urlopen(request, timeout=50).read()
    return html


def store_sql(name, h_id, d_url):
    name = "'" + name + "'"
    d_url = "'" + d_url + "'"
    sql = '''insert into department(name,hospital_id,url) values(''' + name + "," + str(h_id) + "," + d_url + ");"
    print(sql)
    try:
        cursor.execute(sql)
        db.commit()
    except:
        db.rollback()


def get_departmen():
    cursor.execute("select * from hospital")
    lines = cursor.fetchall()
    for line in lines:
        # print line[0], line[8]
        hospital_id = line[0]
        hospital_url = line[8]
        html = get_html(hospital_url)
        pattern = re.compile(r'data-exp=.*?href="(.*?)".*?>(.*?)</a>', re.S)
        items = re.findall(pattern, html)
        for item in items:
            apartment_url = item[0]
            apartment_name = item[1].strip()
            print apartment_name, hospital_id, apartment_url
            store_sql(apartment_name, hospital_id, apartment_url)


get_departmen()
db.close()
