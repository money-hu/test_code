������python http������

�û��ĵ���

1��������
   ָ�������˿ڼ�������
   python fastpy.py 8992

2�����ٱ�дcgi,֧������ʱ�޸�,��������server

   ��fastpy.pyͬһĿ¼��
   ��㽨һ��python �ļ�
   ����:
   example.py:
   #-*- coding:utf-8 -*-
   import sys
   #����һ��ͬ��example��
   #����һ��tt������
   reload(sys)
   sys.setdefaultencoding('utf8')
   FastpyAutoUpdate=True
   class example():
       def tt(self, request, response_head):
           #print request.form
           #print request.getdic
           #fileitem = request.filedic["upload_file"]
           #fileitem.filename
           #fileitem.file.read()
           return "ccb"+request.path

   ����ʸú�����urlΪ http://ip:port/example.tt
   �޸ĺ󱣴棬���ɷ��ʣ���������
   FastpyAutoUpdate ���Կɿ����費��Ҫ�Ȳ���
   FastpyAutoUpdate=true ֧���Ȳ����޸���������
   FastpyAutoUpdate=false ��֧���Ȳ����޸���Ҫ����
   tt�����������������
   request����ʾ��������� Ĭ�ϴ���������
      headers: ͷ�� ���ֵ䣩
      form:  post����������form�� ���ֵ䣩
      getdic: url���� ���ֵ䣩
      filedic: form�����ļ� ���ֵ䣩
      rfile: ԭʼhttp content����  ���ַ�����
      action: python�ļ��� (����Ϊexample)
      method: ��������    ������Ϊtt��
      command:  ��get or post��
      path: url ���ַ�����
      http_version: http�汾�� ��http 1.1��
   response_head: ��ʾresponse���ݵ�ͷ��
      �������Ҫ������gzipѹ��
      ������ͷ��
      response_head["Content-Encoding"] = "gzip"

3�������ļ�
   Ĭ�Ͼ�̬�ļ�(����html��js��css��ͼƬ���ļ���)����static�ļ�����
   html��js��css���Զ�ѹ������
   �����a.jpg�ŵ�static�ļ�����
   ���ʵ�urlΪ http://ip:port/static/a.jpg
   ֧��etag �ͻ��˻��湦��
   (server ʹ��sendfile�����ļ����ͣ���ռ�ڴ��ҿ���)

4��֧����ҳģ���д
   ����һ��ģ�� template.html
   <HTML>
       <HEAD><TITLE>$title</TITLE></HEAD>
       <BODY>
           $contents
       </BODY>
   </HTML>

   ���Ӧ�ĺ�����
   def template(request,response_head):
       t = Template(file="template.html")
       t.title  = "my title"
       t.contents  = "my contents"
       return str(t)
   ģ��ʵ��ʹ����python�����Cheetah��Դģ��,
   ����ԼΪwebpy django thinkphp��ģ���10������:
   http://my.oschina.net/whp/blog/112296


5�����ģʽ��
     ÿ���߳�һ��ʵ����
         fastpy�Ƕ�������ٶ��̵߳�ģʽ��ÿ���߳�һ��example �����.

     ÿ������һ��ʵ����
         �������ĳ����������һ������ֻ����һ����
         ����ʹ�õ���ģʽ

     ���н���һ��ʵ��:
         ��Ϊfastpy�Ƕ���̵ģ�����������н��������߳�Ҳֻʹ��һ������
         ����ֱ��ʹ��python�Ķ���̽ӿ�
         mgr = multiprocessing.Manager()
         ip_dic = mgr.dict()
         ����ÿ�����̵�ÿ���߳��ﶼ�������ip_dic����

