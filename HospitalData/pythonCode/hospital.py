# -*- coding:utf-8 -*-
import re
import urllib2

import MySQLdb

db = MySQLdb.connect("localhost", "root", "******", "guahao", charset="utf8")
cursor = db.cursor()
baseUrl = 'http://www.guahao.com/hospital/areahospitals?sort=region_sort&ipIsShanghai=false&fg=0&c=%E4%B8%8D%E9%99%90&ht=all&q=&p=%E4%B8%8A%E6%B5%B7&ci=all&hk=all&o=all&pi=2&hl=33&pageNo='


def store_sql(name, introduction, address, tel, url):
    name = "'" + name + "'"
    introduction = "'" + introduction + "'"
    address = "'" + address + "'"
    tel = "'" + tel + "'"
    url = "'" + url + "'"
    sql = '''insert into hospital(name,introduction,address,tel,city,url)
    values(''' + name + "," + introduction + "," + address + "," + tel + "," + "'sh'" + ',' + url + ");"
    print(sql)
    try:
        cursor.execute(sql)
        db.commit()
    except:
        db.rollback()


def get_html(url):
    user_agent = 'Mozilla/4.0 (compatible; MSIE 5.5; Windows NT)'
    headers = {'User-Agent': user_agent}
    request = urllib2.Request(url, headers=headers)
    html = urllib2.urlopen(request, timeout=50).read()
    return html


def get_introduction(url):
    ht = get_html(url)
    pattern3 = re.compile(r'class="introduction-content".*?<p>(.*?)</p>', re.S)
    item3 = re.findall(pattern3, ht)
    introduction = str(item3[0])
    introduction = introduction.replace(
        '<p style="margin: 0px; padding: 0px 0px 10px; border: none; line-height: 24px; text-indent: 2em; color: rgb(80  80  80); font-family: Arial  Helvetica  sans-serif  宋体; font-size: 14px; text-align: center;">  &nbsp;',
        ' ')
    introduction = introduction.replace('&ldquo;', '“')
    introduction = introduction.replace('&rdquo;', '”')
    introduction = introduction.replace('</div>', '  ')
    introduction = introduction.replace('<div>', '  ')
    introduction = introduction.replace('<p>', '  ')
    introduction = introduction.replace('&nbsp;', '  ')
    introduction = str(introduction.lstrip())
    return introduction


def get_hospital():
    for i in range(1, 9):
        url = baseUrl + str(i)
        html = get_html(url)

        pattern = re.compile(
            r'<div class="hos.*?href="(.*?)".*?title="(.*?)".*?tel.*?title="(.*?)".*?addr.*?title="(.*?)">.*?</span>',
            re.S)
        items = re.findall(pattern, html)
        for item in items:
            detailurl = item[0]
            name = item[1]
            tel = item[2]
            address = item[3]
            hospital_detail = get_html(detailurl)
            pattern2 = re.compile(r'class="about">.*?href="(.*?)">.*?</a>', re.S)
            items2 = re.findall(pattern2, hospital_detail)
            if 'introduction' in items2[0]:
                introduction = get_introduction(items2[0])
            else:
                introduction = 'null'
            print name, tel, address, introduction, detailurl
            store_sql(name, introduction, address, tel, detailurl)

    db.close()


get_hospital()
