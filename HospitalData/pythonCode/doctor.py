# -*- coding:utf-8 -*-
import re
import sys
import urllib2

import MySQLdb

reload(sys)
sys.setdefaultencoding('utf8')

db = MySQLdb.connect("localhost", "root", "******", "guahao", charset="utf8")
cursor = db.cursor()


def get_html(url):
    enable_proxy = True
    proxy_handler = urllib2.ProxyHandler({"http": 'http://120.195.192.224:80'})
    null_proxy_handler = urllib2.ProxyHandler({})
    if enable_proxy:
        opener = urllib2.build_opener(proxy_handler)
    else:
        opener = urllib2.build_opener(null_proxy_handler)
    urllib2.install_opener(opener)
    user_agent = 'Mozilla/4.0 (compatible; MSIE 5.5; Windows NT)'
    headers = {'User-Agent': user_agent}
    request = urllib2.Request(url, headers=headers)
    html = urllib2.urlopen(request, timeout=1500).read()
    return html


def store_sql(name, introduction, goodat, d_id):
    name = "'" + name + "'"
    introduction = "'" + introduction + "'"
    goodat = "'" + goodat + "'"
    sql = '''insert into doctor(name,introduction,specialty,department_id) values(''' + name + "," + introduction + "," + goodat + "," + str(
        d_id) + ");"
    print(sql)
    try:
        cursor.execute(sql)
        db.commit()
    except:
        db.rollback()


def get_doctor():
    cursor.execute("select * from department")
    lines = cursor.fetchall()
    for it in range(5945, 7733):
        line = lines[it]
        department_id = line[0]
        department_url = line[3]
        # print department_id, department_url
        html = get_html(department_url)
        pattern = re.compile(r'<a class="img" href="(.*?)".*?target', re.S)
        items = re.findall(pattern, html)
        for item in items:
            if 'this.doctorUuid' in item:
                print 'done'
            else:
                #print item
                html2 = get_html(item)
                pattern2 = re.compile(r'class="detail w.*?<strong>(.*?)</strong>', re.S)
                items2 = re.findall(pattern2, html2)
                d_name = items2[0]
                pattern2 = re.compile(r'class="goodat".*?<span>(.*?)</', re.S)
                items2 = re.findall(pattern2, html2)
                d_goodat = items2[0]
                pattern2 = re.compile(r'class="about".*?data-description="(.*?)">', re.S)
                items2 = re.findall(pattern2, html2)
                if items2.__len__() > 0:
                    d_intro = items2[0]
                else:
                    d_intro = 'null'
                #print d_name, d_goodat, d_intro
                store_sql(d_name, d_intro, d_goodat, department_id)


get_doctor()
# def test():
#     cursor.execute("select * from department")
#     lines = cursor.fetchall()
#     for it in range(0, 40):
#         line = lines[it]
#         print line[0], line[3]
#         # for line in lines:
#         #     department_id = line[0]
#         #     department_url = line[3]
#         #     print department_id , department_url
#

# test()
db.close()
