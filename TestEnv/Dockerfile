FROM debian

WORKDIR /

RUN apt-get update && apt-get install -y \
    systemctl \
    snmp \
    snmpd

COPY docker-entrypoint.sh /usr/local/bin/
COPY snmpd.conf /etc/snmp/snmpd.conf

RUN systemctl enable snmpd

EXPOSE 161/udp

ENTRYPOINT ["docker-entrypoint.sh"]
CMD [ "snmpd" ]